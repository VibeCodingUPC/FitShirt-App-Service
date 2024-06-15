using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Publishing.Repositories;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<IReadOnlyCollection<Post>> GetPostsByUserId(int userId);
    Task<IReadOnlyCollection<Post>> SearchByFiltersAsync (int? categoryId, int? colorId);
}