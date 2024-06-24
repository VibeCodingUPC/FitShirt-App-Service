using FitShirt.Domain.Security.Services;

namespace FitShirt.Application.Security.Features.CommandServices;

public class EncryptService : IEncryptService
{
    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string passwordHashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHashed);
    }
}