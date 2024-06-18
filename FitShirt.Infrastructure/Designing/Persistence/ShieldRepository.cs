using FitShirt.Domain.Designing.Models.Entities;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace FitShirt.Infrastructure.Designing.Persistence;

public class ShieldRepository : BaseRepository<Shield>, IShieldRepository
{
    public ShieldRepository(FitShirtDbContext context) : base(context)
    {
    }

    public async Task<Shield?> GetShieldByIdAsync(int id)
    {
        return await _context.Shields
            .Where(shield => shield.IsEnable && shield.Id == id)
            .FirstOrDefaultAsync();
    }
}