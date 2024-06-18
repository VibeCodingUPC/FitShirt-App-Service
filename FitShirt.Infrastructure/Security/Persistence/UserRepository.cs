using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;

namespace FitShirt.Infrastructure.Security.Persistence;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(FitShirtDbContext context) : base(context)
    {
    }
}