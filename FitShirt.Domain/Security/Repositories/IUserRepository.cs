using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Security.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetDetailedUserInformationAsync(int id);
    Task<IReadOnlyCollection<User?>> GetUsersByRoleAsync(UserRoles userRoles);
    Task<IReadOnlyCollection<User?>> GetAllDetailedUserInformationAsync();
}