using AutoMapper;
using FitShirt.Application.Purchasing.Exceptions;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Models.Commands;
using FitShirt.Domain.Purchasing.Models.Entities;
using FitShirt.Domain.Purchasing.Models.Responses;
using FitShirt.Domain.Purchasing.Repositories;
using FitShirt.Domain.Purchasing.Services;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Application.Purchasing.Features.CommandServices;

public class PurchaseCommandService:IPurchaseCommandService
{

    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly ISizeRepository _sizeRepository;
    
    private readonly IMapper _mapper;

    public PurchaseCommandService(IPurchaseRepository purchaseRepository, IUserRepository userRepository,
        IMapper mapper, IPostRepository postRepository, ISizeRepository sizeRepository)
    {
        _purchaseRepository = purchaseRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _postRepository = postRepository;
        _sizeRepository = sizeRepository;
    }

    public async Task<PurchaseResponse> Handle(CreatePurchaseCommand command)
    {
        var purchaseEntity = _mapper.Map<CreatePurchaseCommand, Purchase>(command);
        
        purchaseEntity.PurchaseDate = DateTime.Now;

        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), command.UserId);
        }
        purchaseEntity.User = user;
        
        List<Item> items = new List<Item>();
        foreach (var itemRequest in command.Items)
        {
            var post = await _postRepository.GetByIdAsync(itemRequest.PostId);
            var size = await _sizeRepository.GetByIdAsync(itemRequest.SizeId);

            if (post == null)
            {
                throw new NotFoundEntityIdException(nameof(Post), itemRequest.PostId);
            }
            if (size == null)
            {
                throw new NotFoundEntityIdException(nameof(Size), itemRequest.SizeId);
            }

            if (itemRequest.Quantity > post.Stock)
            {
                throw new InsufficientStockException();
            }

            post.Stock -= itemRequest.Quantity;
            await _postRepository.ModifyAsync(post);

            var item = new Item
            {
                Post = post,
                Size = size,
                Quantity = itemRequest.Quantity
            };
            
            items.Add(item);
        }

        purchaseEntity.Items = items;

        await _purchaseRepository.SaveAsync(purchaseEntity);
        
        var purchaseResponse = _mapper.Map<PurchaseResponse>(purchaseEntity);
        
        return purchaseResponse;
    }
    
}