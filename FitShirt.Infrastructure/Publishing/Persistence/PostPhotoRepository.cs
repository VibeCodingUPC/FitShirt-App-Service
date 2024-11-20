using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Publishing.Persistence;

public class PostPhotoRepository : BaseRepository<PostPhoto>, IPostPhotoRepository
{
    public PostPhotoRepository(FitShirtDbContext context) : base(context)
    {
    }

    public async Task<PostPhoto?> GetPostPhotoByPostId(int postId)
    {
        return await _context.PostPhotos
            .Where(pp => pp.IsEnable && pp.PostId == postId)
            .FirstOrDefaultAsync();
    }
}