using FitShirt.Domain.OrderManagement.Models.Queries;
using FitShirt.Domain.OrderManagement.Models.Responses;

namespace FitShirt.Domain.OrderManagement.Services;

public interface IDesignOrderQueryService
{
    Task<IReadOnlyCollection<DesignOrderResponse>> Handle(GetAllDesignOrdersQuery query);
    Task<IReadOnlyCollection<DesignOrderResponse>> Handle(GetDesignOrdersBySellerId query);
    Task<DesignOrderResponse> Handle(GetDesignOrderById query);
}