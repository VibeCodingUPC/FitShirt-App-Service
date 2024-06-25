using FitShirt.Domain.Security.Models.Entities;
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
        return await _context.Roles.FirstOrDefaultAsync(role => role.Name == "Client");
    }

    public async Task<Role?> GetAdminRoleAsync()
    {
        return await _context.Roles.FirstOrDefaultAsync(role => role.Name == "Admin");
    }
}