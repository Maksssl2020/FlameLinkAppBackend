using System.Security.Claims;
using System.Text;
using DatingAppProject.Data;
using DatingAppProject.Entities;
using DatingAppProject.Entities.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DatingAppProject.extensions;

public static class IdentityServiceExtension {

    public static IServiceCollection
        AddIdentityServices(this IServiceCollection services, IConfiguration configuration){
        services.AddIdentityCore<AppUser>(opt => {
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddEntityFrameworkStores<DataContext>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
            var tokenSecret = configuration["TokenSecretKey"] ?? throw new Exception("Cannot find token secret key.");
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role
            };
        });
        
        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
            .AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
        
        return services;
    }
}