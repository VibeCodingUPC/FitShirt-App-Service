using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Purchasing.Repositories;

public interface IPurchaseRepository : IBaseRepository<Purchase>
{
    Task<IReadOnlyCollection<Purchase>> GetPurchasesByUserId(int userId);
    Task<IReadOnlyList<Purchase>> GetAllPurchasesAsync();
    Task<Purchase?> GetPurchaseByIdAsync(int id);

}