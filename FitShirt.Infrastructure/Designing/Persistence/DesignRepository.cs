using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;

namespace FitShirt.Infrastructure.Designing.Persistence;

public class DesignRepository : BaseRepository<Design>, IDesignRepository
{
    public DesignRepository(FitShirtDbContext context) : base(context)
    {
    }
}