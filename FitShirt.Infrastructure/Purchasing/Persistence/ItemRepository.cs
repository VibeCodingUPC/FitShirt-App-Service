using FitShirt.Domain.Purchasing.Models.Entities;
using FitShirt.Domain.Purchasing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;

namespace FitShirt.Infrastructure.Purchasing.Persistence;

public class ItemRepository : BaseRepository<Item>, IItemRepository
{
    public ItemRepository(FitShirtDbContext context) : base(context)
    {
    }

}