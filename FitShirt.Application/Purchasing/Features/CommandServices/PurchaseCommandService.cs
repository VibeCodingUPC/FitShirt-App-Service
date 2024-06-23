using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Models.Commands;
using FitShirt.Domain.Purchasing.Models.Responses;
using FitShirt.Domain.Purchasing.Repositories;
using FitShirt.Domain.Purchasing.Services;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;

namespace FitShirt.Application.Purchasing.Features.CommandServices;

public class PurchaseCommandService:IPurchaseCommandService
{

    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public PurchaseCommandService(IPurchaseRepository purchaseRepository, IUserRepository userRepository,
        IMapper mapper)
    {
        _purchaseRepository = purchaseRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PurchaseResponse> Handle(CreatePurchaseCommand command)
    {
        var purchaseEntity = _mapper.Map<CreatePurchaseCommand, Purchase>(command);

        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), command.UserId);
        }

        purchaseEntity.User = user;

        await _purchaseRepository.SaveAsync(purchaseEntity);
        
        var purchaseResponse = _mapper.Map<PurchaseResponse>(purchaseEntity);
        
        return purchaseResponse;
    }
    
}