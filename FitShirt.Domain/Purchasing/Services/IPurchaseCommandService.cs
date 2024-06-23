using FitShirt.Domain.Purchasing.Models.Commands;
using FitShirt.Domain.Purchasing.Models.Responses;

namespace FitShirt.Domain.Purchasing.Services;

public interface IPurchaseCommandService
{
    Task<PurchaseResponse> Handle(CreatePurchaseCommand command);

}