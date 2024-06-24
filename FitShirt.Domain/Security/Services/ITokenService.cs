using FitShirt.Domain.Security.Models.Aggregates;

namespace FitShirt.Domain.Security.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    Task<int?> ValidateToken(string token);
}