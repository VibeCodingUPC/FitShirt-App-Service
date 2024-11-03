using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Publishing.Repositories;

public interface IPostPhotoRepository : IBaseRepository<PostPhoto>
{
    Task<PostPhoto?> GetPostPhotoByPostId(int postId);
}