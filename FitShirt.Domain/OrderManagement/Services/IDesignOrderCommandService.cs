using FitShirt.Domain.OrderManagement.Models.Commands;
using FitShirt.Domain.OrderManagement.Models.Responses;

namespace FitShirt.Domain.OrderManagement.Services;

public interface IDesignOrderCommandService
{
    Task<DesignOrderResponse> Handle(CreateDesignOrderCommand command);
}