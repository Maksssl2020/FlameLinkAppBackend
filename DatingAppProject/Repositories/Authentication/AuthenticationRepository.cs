using System.Security.Authentication;
using AutoMapper;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Entities.User;
using DatingAppProject.Exceptions;
using DatingAppProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories.Authentication;

public class AuthenticationRepository(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) : IAuthenticationRepository {
    
    public async Task<AuthenticationDto> Register(RegisterRequestDto registerRequest){
        if (await IsFieldTaken("username", registerRequest.Username)) {
            throw new CustomAuthenticationException("Username already taken.");
        }
        
        if (await IsFieldTaken("email", registerRequest.Email)) {
            throw new CustomAuthenticationException("Email already taken.");
        }

        var appUser = mapper.Map<AppUser>(registerRequest);
        var result = await userManager.CreateAsync(appUser, registerRequest.Password);
        
        if (!result.Succeeded) {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            throw new CustomAuthenticationException($"Registration failed: {errors}");
        }
        
        var roleResult = await userManager.AddToRoleAsync(appUser, "User");
        if (roleResult.Succeeded)
            return await Login(new LoginRequestDto {
                Username = registerRequest.Username,
                Password = registerRequest.Password
            });
        {
            var errors = string.Join(",", roleResult.Errors.Select(e => e.Description));
            throw new CustomAuthenticationException($"Registration failed: {errors}");
        }

    }

    public async Task RegisterAdmin(RegisterAdminRequestDto registerAdminRequest){
        var appUser = new AppUser {
            UserName = registerAdminRequest.Username,
            Email = registerAdminRequest.Email,
            FirstName = "Admin",
            LastName = "User",
            Gender = "None",
            City = "Unknown",
            Country = "Unknown",
            Preference = "None",
            Interests = "None",
        };
        var result = await userManager.CreateAsync(appUser, registerAdminRequest.Password);
        
        
        if (!result.Succeeded) {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            throw new CustomAuthenticationException($"Registration failed: {errors}");
        }
        
        await userManager.AddToRoleAsync(appUser, "Admin");
    }

    public async Task<AuthenticationDto> Login(LoginRequestDto loginRequest){
        var foundUser = await userManager.Users
            .FirstOrDefaultAsync(user => user.UserName == loginRequest.Username);

        if (foundUser?.UserName == null || !await userManager.CheckPasswordAsync(foundUser, loginRequest.Password)) {
            throw new CustomAuthenticationException("Invalid credentials.");
        }

        var userRoles = await userManager.GetRolesAsync(foundUser);
        
        return new AuthenticationDto {
            UserId = foundUser.Id,
            Username = foundUser.UserName,
            Email = foundUser.Email,
            AccessToken = await tokenService.GenerateToken(foundUser),
            Roles = userRoles
        };
    }
    
    public async Task<bool> IsFieldTaken(string field, string value){
        Console.WriteLine($"Checking if {field} with value {value} is taken.");
        return field switch {
            "username" => await userManager.Users.AnyAsync(user => user.UserName == value),
            "email" => await userManager.Users.AnyAsync(user => user.Email == value),
            _ => throw new ArgumentException("Invalid field name.")
        };
    }
}