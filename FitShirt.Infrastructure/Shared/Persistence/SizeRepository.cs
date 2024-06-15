using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;

namespace FitShirt.Infrastructure.Shared.Persistence;

public class SizeRepository : BaseRepository<Size>, ISizeRepository
{
    public SizeRepository(FitShirtDbContext context) : base(context)
    {
    }
}