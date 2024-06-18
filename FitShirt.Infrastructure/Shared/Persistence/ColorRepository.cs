using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;

namespace FitShirt.Infrastructure.Shared.Persistence;

public class ColorRepository : BaseRepository<Color>, IColorRepository
{
    public ColorRepository(FitShirtDbContext context) : base(context)
    {
    }
}