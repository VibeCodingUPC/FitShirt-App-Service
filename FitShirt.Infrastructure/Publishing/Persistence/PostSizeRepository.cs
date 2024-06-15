using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Publishing.Persistence;

public class PostSizeRepository : BaseRepository<PostSize>, IPostSizeRepository
{
    public PostSizeRepository(FitShirtDbContext context) : base(context)
    {
    }

    public async Task<bool> DeleteByPostIdAsync(int postId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var postSizesToDelete = _context.PostSizes
                .Where(ps => ps.PostId == postId)
                .ToList();
            
            _context.PostSizes.RemoveRange(postSizesToDelete);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}