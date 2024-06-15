using FitShirt.Domain.Designing.Models.Entities;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;

namespace FitShirt.Infrastructure.Designing.Persistence;

public class ShieldRepository : BaseRepository<Shield>, IShieldRepository
{
    public ShieldRepository(FitShirtDbContext context) : base(context)
    {
    }
}