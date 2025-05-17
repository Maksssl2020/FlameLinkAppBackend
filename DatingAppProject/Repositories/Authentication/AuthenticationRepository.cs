using System.Security.Authentication;
using AutoMapper;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Exceptions;
using DatingAppProject.Repositories.InterestRepository;
using DatingAppProject.Repositories.ProfileRepository;
using DatingAppProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories.Authentication;

public class AuthenticationRepository(IInterestRepository interestRepository, UserManager<AppUser> userManager, ITokenService tokenService, IUserProfileRepository userProfileRepository, IMapper mapper) : IAuthenticationRepository {
    
    public async Task<AuthenticationDto> Register(RegisterRequestDto registerRequest){
        var appUser = mapper.Map<AppUser>(registerRequest);
        
        if (registerRequest.Interests.Count != 0) {
            var foundInterests = await interestRepository.GetAllByNames(registerRequest.Interests);
            appUser.Interests = foundInterests;
        }

        
        var result = await userManager.CreateAsync(appUser, registerRequest.Password);
        
        if (!result.Succeeded) {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            throw new CustomAuthenticationException($"Registration failed: {errors}");
        }
        
        var roleResult = await userManager.AddToRoleAsync(appUser, "User");
        if (!roleResult.Succeeded) {
            var errors = string.Join(",", roleResult.Errors.Select(e => e.Description));
            throw new CustomAuthenticationException($"Registration failed: {errors}");
        }
        
        await userProfileRepository.SaveProfile(appUser, registerRequest.LookingFor);
        
        if (await userProfileRepository.SaveAll()) {
            return await Login(new LoginRequestDto {
                Username = registerRequest.Username,
                Password = registerRequest.Password
            });
        }
        
        throw new CustomAuthenticationException($"Registration failed: {result.Errors.First().Description}");
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
            Interests = [],
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
        return field switch {
            "username" => await userManager.Users.AnyAsync(user => user.UserName == value),
            "email" => await userManager.Users.AnyAsync(user => user.Email == value),
            _ => throw new ArgumentException("Invalid field name.")
        };
    }
}