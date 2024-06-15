using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;

namespace FitShirt.Infrastructure.Security.Persistence;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(FitShirtDbContext context) : base(context)
    {
    }
}