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

    public async Task<IReadOnlyCollection<Post>> GetShirtsAsync()
    {
        return await _context.Posts
            .Where(p => p.IsEnable)
            .Include(p => p.PostPhoto)
            .ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _context.Posts
            .Where(post => post.IsEnable && post.Id == id)
            .Include(post => post.Category)
            .Include(post => post.Color)
            .Include(p => p.PostPhoto)
            .Include(post => post.User)
                .ThenInclude(user => user.Role)
            .Include(post => post.PostSizes)
                .ThenInclude(postSize => postSize.Size)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<Post>> GetPostsByUserIdAsync(int userId)
    {
        return await _context.Posts
            .Where(post => post.UserId == userId && post.IsEnable == true)
            .Include(p => p.PostPhoto)
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
            .Include(p => p.PostPhoto)
            .ToListAsync();
    }

    public async Task<Post?> GetPostByName(string name)
    {
        return await _context.Posts
            .Where(post => post.Name == name)
            .Include(p => p.PostPhoto)
            .FirstOrDefaultAsync();
    }
}