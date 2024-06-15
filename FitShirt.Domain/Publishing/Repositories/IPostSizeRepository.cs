using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Publishing.Repositories;

public interface IPostSizeRepository : IBaseRepository<PostSize>
{
    Task<bool> DeleteByPostIdAsync(int postId);
}