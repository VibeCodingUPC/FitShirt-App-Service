using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Publishing.Repositories;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<IReadOnlyCollection<Post>> GetShirtsAsync();
    Task<Post?> GetPostByIdAsync(int id);
    Task<IReadOnlyCollection<Post>> GetPostsByUserIdAsync(int userId);
    Task<IReadOnlyCollection<Post>> SearchByFiltersAsync (int? categoryId, int? colorId);
    Task<Post?> GetPostByName(string name);
}