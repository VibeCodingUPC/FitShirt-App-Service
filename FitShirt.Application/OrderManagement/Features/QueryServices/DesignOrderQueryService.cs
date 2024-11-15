using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Domain.OrderManagement.Models.Aggregates;
using FitShirt.Domain.OrderManagement.Models.Queries;
using FitShirt.Domain.OrderManagement.Models.Responses;
using FitShirt.Domain.OrderManagement.Repositories;
using FitShirt.Domain.OrderManagement.Services;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;

namespace FitShirt.Application.OrderManagement.Features.QueryServices;

public class DesignOrderQueryService : IDesignOrderQueryService
{
    private readonly IDesignOrderRepository _designOrderRepository;
    private readonly IDesignRepository _designRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public DesignOrderQueryService(IDesignOrderRepository designOrderRepository, IDesignRepository designRepository, IUserRepository userRepository, IMapper mapper)
    {
        _designOrderRepository = designOrderRepository;
        _designRepository = designRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<DesignOrderResponse>> Handle(GetAllDesignOrdersQuery query)
    {
        var data = await _designOrderRepository.GetAllAsync();
        
        if (data.Count==0)
        {
            throw new NoEntitiesFoundException(nameof(DesignOrder));
        }
        
        var result = _mapper.Map<List<DesignOrderResponse>>(data);
        return result;
    }

    public async Task<IReadOnlyCollection<DesignOrderResponse>> Handle(GetDesignOrdersBySellerId query)
    {
        var user = await _userRepository.GetByIdAsync(query.SellerId);

        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), query.SellerId);
        }

        var designOrderList = await _designOrderRepository.GetDesignsBySellerIdAsync(query.SellerId);
        var designOrderListResponse = _mapper.Map<List<DesignOrderResponse>>(designOrderList);
        return designOrderListResponse;
    }

    public async Task<DesignOrderResponse> Handle(GetDesignOrderById query)
    {
        var designOrder = await _designOrderRepository.GetByIdAsync(query.Id);

        if (designOrder == null)
        {
            throw new NotFoundEntityIdException(nameof(DesignOrder), query.Id);
        }

        if (designOrder.User.Id != query.SellerId)
        {
            throw new UnauthorizedAccessException("Not authorized access");
        }

        var designOrderResponse = _mapper.Map<DesignOrderResponse>(designOrder);
        return designOrderResponse;
    }
}