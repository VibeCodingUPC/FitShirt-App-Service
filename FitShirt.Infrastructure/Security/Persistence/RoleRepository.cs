using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Security.Persistence;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(FitShirtDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetClientRoleAsync()
    {
        return await _context.Roles.FirstOrDefaultAsync(role => role.Equals(UserRoles.CLIENT));
    }

    public async Task<Role?> GetAdminRoleAsync()
    {
        return await _context.Roles.FirstOrDefaultAsync(role => role.Equals(UserRoles.ADMIN));
    }

    public async Task<Role?> GetSellerRoleAsync()
    {
        return await _context.Roles.FirstOrDefaultAsync(role => role.Equals(UserRoles.SELLER));
    }

    public async Task<IReadOnlyCollection<Role>> GetPublicRolesAsync()
    {
        return await _context.Roles.Where(role => !role.Equals(UserRoles.ADMIN)).ToListAsync();
    }

    public async Task<Role?> GetRoleByNameAsync(UserRoles roleName)
    {
        return await _context.Roles
            .Where(r => r.Name == roleName)
            .FirstOrDefaultAsync();
    }
}