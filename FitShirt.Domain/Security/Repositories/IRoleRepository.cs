using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Security.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetClientRoleAsync();
    Task<Role?> GetAdminRoleAsync();
}