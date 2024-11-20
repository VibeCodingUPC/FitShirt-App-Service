using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Models.Queries;
using FitShirt.Domain.Purchasing.Models.Responses;
using FitShirt.Domain.Purchasing.Repositories;
using FitShirt.Domain.Purchasing.Services;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;

namespace FitShirt.Application.Purchasing.Features.QueryServices;

public class PurchaseQueryService:IPurchaseQueryService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public PurchaseQueryService(IPurchaseRepository purchaseRepository, IUserRepository userRepository, IMapper mapper)
    {
        _purchaseRepository = purchaseRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<PurchaseResponse?> Handle(GetPurchaseByIdQuery query)
    {
        var data = await _purchaseRepository.GetPurchaseByIdAsync(query.Id);

        if (data == null)
        {
            throw new NotFoundEntityIdException(nameof(Purchase), query.Id);
        }

        var purchaseResponse = _mapper.Map<PurchaseResponse>(data);
        return purchaseResponse;
    }

    public async Task<IReadOnlyCollection<PurchaseResponse>> Handle(GetAllPurchasesQuery query)
    {
        var data = await _purchaseRepository.GetAllPurchasesAsync();
        if (data.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(Purchase));
        }

        var result = _mapper.Map<List<PurchaseResponse>>(data);
        return result;
    }

    public async Task<IReadOnlyCollection<PurchaseResponse>> Handle(GetPurchaseByUserIdQuery query)
    {
        var user = await _userRepository.GetDetailedUserInformationAsync(query.UserId);
        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), query.UserId);
        }

        var purchaseList = await _purchaseRepository.GetPurchasesByUserId(query.UserId);
        var purchaseListResponse = _mapper.Map<List<PurchaseResponse>>(purchaseList);
        return purchaseListResponse;
    }
}