using FitShirt.Domain.Purchasing.Models.Queries;
using FitShirt.Domain.Purchasing.Models.Responses;

namespace FitShirt.Domain.Purchasing.Services;

public interface IPurchaseQueryService
{
    Task<PurchaseResponse?> Handle(GetPurchaseByIdQuery query);
    Task<IReadOnlyCollection<ItemResponse>> Handle(GetAllPurchasesQuery query);
    Task<IReadOnlyCollection<ItemResponse>> Handle(GetPurchaseByUserIdQuery query);
}