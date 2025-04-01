using DatingAppProject.Entities;
using DatingAppProject.Entities.User;

namespace DatingAppProject.Services;

public interface ITokenService {
    Task<string> GenerateToken(AppUser appUser);
}