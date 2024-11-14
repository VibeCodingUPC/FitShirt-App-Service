using FitShirt.Domain.OrderManagement.Models.Aggregates;
using FitShirt.Domain.OrderManagement.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.OrderManagement.Persistence;

public class DesignOrderRepository : BaseRepository<DesignOrder>, IDesignOrderRepository
{
    public DesignOrderRepository(FitShirtDbContext context) : base(context)
    {
    }

    public override async Task<IReadOnlyCollection<DesignOrder>> GetAllAsync()
    {
        return await _context.DesignOrders
            .Where(d => d.IsEnable)
                .Include(d => d.User)
                    .ThenInclude(u => u.Role)
                .Include(d => d.Design)
                .Include(d => d.Design.PrimaryColor)
                .Include(d => d.Design.SecondaryColor)
                .Include(d => d.Design.TertiaryColor)
                .Include(d => d.Design.Shield).Include(d => d.Design.User)
                    .ThenInclude(u => u.Role)
            .ToListAsync();
    }

    public override async Task<DesignOrder?> GetByIdAsync(int id)
    {
        return await _context.DesignOrders
            .Where(d => d.IsEnable && d.Id==id)
                .Include(d => d.User)
                    .ThenInclude(u => u.Role)
            .Include(d => d.Design)
            .ThenInclude(design => design.PrimaryColor)
            .Include(d => d.Design.SecondaryColor)
            .Include(d => d.Design.TertiaryColor)
            .Include(d => d.Design.Shield)
            .Include(d => d.Design.User)
                .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<DesignOrder>> GetDesignsBySellerIdAsync(int sellerId)
    {
        return await _context.DesignOrders
            .Where(d => d.IsEnable && d.UserId == sellerId)
            .Include(d => d.User)
                .ThenInclude(u => u.Role)
            .Include(d => d.Design)
            .Include(d => d.Design.PrimaryColor)
            .Include(d => d.Design.SecondaryColor)
            .Include(d => d.Design.TertiaryColor)
            .Include(d => d.Design.Shield)
            .Include(d => d.Design.User)
                .ThenInclude(u => u.Role)
            .ToListAsync();
    }
}