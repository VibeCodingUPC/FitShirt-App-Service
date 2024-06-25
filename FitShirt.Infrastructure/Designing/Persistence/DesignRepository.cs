using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Designing.Persistence;

public class DesignRepository : BaseRepository<Design>, IDesignRepository
{
    public DesignRepository(FitShirtDbContext context) : base(context)
    {
    }

    public async Task<Design?> GetDesignByIdAsync(int id)
    {
        return await _context.Designs
            .Where(design => design.IsEnable && design.Id == id)
            .Include(design => design.PrimaryColor)
            .Include(design => design.SecondaryColor)
            .Include(design => design.TertiaryColor)
            .Include(design => design.Shield)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<Design>> GetDesignByUserIdAsync(int userId)
    {
        return await _context.Designs
            .Where(design => design.UserId == userId && design.IsEnable == true)
            .ToListAsync();

    }

    public async Task<Design?> GetDesignByName(string name)
    {
        return await _context.Designs
            .Where(design => design.Name == name)
            .FirstOrDefaultAsync();
    }
}