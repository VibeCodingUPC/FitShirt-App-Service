using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Publishing.Persistence;

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    public PostRepository(FitShirtDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Post>> GetPostsByUserId(int userId)
    {
        return await _context.Posts
            .Where(post => post.UserId == userId && post.IsEnable == true)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Post>> SearchByFiltersAsync(int? categoryId, int? colorId)
    {
        return await _context.Posts
            .Where(post =>
                (categoryId==null || post.CategoryId == categoryId) &&
                (colorId==null || post.ColorId == colorId) &&
                post.IsEnable
            )
            .ToListAsync();
    }
}