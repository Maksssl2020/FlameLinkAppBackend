using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DatingAppProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DatingAppProject.Services;

public class TokenService(IConfiguration configuration, UserManager<AppUser> userManager) : ITokenService {
    public async Task<string> GenerateToken(AppUser appUser){
        var tokenSecret = configuration["TokenSecretKey"] ?? throw new Exception("Cannot find token secret key.");

        if (tokenSecret.Length < 64) {
            throw new Exception("Token secret key must be at least 64 characters.");
        }
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));

        if (appUser.UserName == null || appUser.Email == null) {
            throw new Exception("There is no user!");
        }

        var claims = new List<Claim>() {
            new(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new(ClaimTypes.Name, appUser.UserName),
            new(ClaimTypes.Email, appUser.Email),
        };
        
        var roles = await userManager.GetRolesAsync(appUser);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}