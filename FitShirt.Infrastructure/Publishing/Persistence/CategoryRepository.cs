using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;

namespace FitShirt.Infrastructure.Publishing.Persistence;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FitShirtDbContext context) : base(context)
    {
    }
}