using FitShirt.Domain.Purchasing.Models.Queries;
using FitShirt.Domain.Purchasing.Models.Responses;

namespace FitShirt.Domain.Purchasing.Services;

public interface IPurchaseQueryService
{
    Task<PurchaseResponse?> Handle(GetPurchaseByIdQuery query);
    Task<IReadOnlyCollection<PurchaseResponse>> Handle(GetAllPurchasesQuery query);
    Task<IReadOnlyCollection<PurchaseResponse>> Handle(GetPurchaseByUserIdQuery query);
}