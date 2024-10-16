using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Purchasing.Persistence;

public class PurchaseRepository : BaseRepository<Purchase>, IPurchaseRepository
{
    public PurchaseRepository(FitShirtDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Purchase>> GetPurchasesByUserId(int userId)
    {
        return await _context.Purchases
            .Where(purchase => purchase.IsEnable && purchase.UserId == userId)
            .Include(purchase => purchase.User)
                .ThenInclude(user => user.Role)
            .Include(purchase => purchase.Items)
            .ThenInclude(item => item.Size)
            .Include(purchase => purchase.Items)
            .ThenInclude(item => item.Post)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Purchase>> GetAllPurchasesAsync()
    {
        return await _context.Purchases
            .Where(purchase => purchase.IsEnable)
            .Include(purchase => purchase.User)
            .Include(purchase => purchase.Items)
            .ThenInclude(item => item.Size)
            .Include(purchase => purchase.Items)
            .ThenInclude(item => item.Post)
            .ToListAsync();
    }

    public async Task<Purchase?> GetPurchaseByIdAsync(int id)
    {
        return await _context.Purchases
            .Where(purchase => purchase.IsEnable && purchase.Id == id)
            .Include(purchase => purchase.User)
                .ThenInclude(user => user.Role)
            .Include(purchase => purchase.Items)
            .ThenInclude(item => item.Size)
            .Include(purchase => purchase.Items)
            .ThenInclude(item => item.Post)
            .FirstOrDefaultAsync();
    }


}