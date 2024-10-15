using System.Security.Claims;
using System.Text;
using FitShirt.Application.Shared.Constants;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Services;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FitShirt.Application.Security.Features.OutboundServices;

public class TokenService : ITokenService
{
    private readonly string _key = "681fcdef-f04e-4c0c-b91f-977c55b92b56";
    private readonly int _durationInMinutes = 360;

    public string GenerateToken(User user)
    {
        var claims = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            new Claim(CustomClaimTypes.Name, user.Name),
            new Claim(CustomClaimTypes.Lastname, user.Lastname),
            new Claim(CustomClaimTypes.Username, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.PhoneNumber, user.Cellphone),
            new Claim(CustomClaimTypes.Role, user.Role.GetStringName())
        });

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(_durationInMinutes),
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JsonWebTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }

    public async Task<int?> ValidateToken(string token)
    {
        // If token is null or empty
        if (string.IsNullOrEmpty(token))
            // Return null 
            return null;

        // Otherwise, perform validation
        var tokenHandler = new JsonWebTokenHandler();
        var key = Encoding.ASCII.GetBytes(_key);
        try
        {
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });

            var jwtToken = (JsonWebToken)tokenValidationResult.SecurityToken;
            var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sid).Value);
            return userId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}