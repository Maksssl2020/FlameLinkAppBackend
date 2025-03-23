using DatingAppProject.Entities;

namespace DatingAppProject.Services;

public interface ITokenService {
    Task<string> GenerateToken(AppUser appUser);
}