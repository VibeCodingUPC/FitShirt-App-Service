using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Security.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetAdminRoleAsync();
    Task<Role?> GetClientRoleAsync();
    Task<Role?> GetSellerRoleAsync();
    Task<IReadOnlyCollection<Role>> GetPublicRolesAsync();
    Task<Role?> GetRoleByNameAsync(UserRoles roleName);
}