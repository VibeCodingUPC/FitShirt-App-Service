using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Security.Persistence;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(FitShirtDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Where(user => user.Email == email && user.IsEnable == true)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Users
            .Where(user => user.Cellphone == phoneNumber && user.IsEnable == true)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Where(user => user.Username == username && user.IsEnable == true)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetDetailedUserInformationAsync(int id)
    {
        return await _context.Users
            .Where(user => user.Id == id && user.IsEnable)
            .Include(user => user.Role)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<User?>> GetUsersByRoleAsync(UserRoles userRoles)
    {
        return await _context.Users
            .Include(user => user.Role)
            .Where(user => user.Role.Name == userRoles)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<User?>> GetAllDetailedUserInformationAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .ToListAsync();
    }
}