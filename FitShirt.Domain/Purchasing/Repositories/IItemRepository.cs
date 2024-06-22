using FitShirt.Domain.Purchasing.Models.Entities;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Purchasing.Repositories;

public interface IItemRepository : IBaseRepository<Item>
{
    Task<Item?> GetItemByIdAsync(int id);
}
    
