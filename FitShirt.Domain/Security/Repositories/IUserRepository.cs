using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Security.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByName(string name);
}