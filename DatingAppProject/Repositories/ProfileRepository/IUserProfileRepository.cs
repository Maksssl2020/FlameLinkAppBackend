using DatingAppProject.DTO;
using DatingAppProject.Entities;

namespace DatingAppProject.Repositories.ProfileRepository;

public interface IUserProfileRepository {
    Task SaveProfile(AppUser registeredUser, string lookingFor);
    Task<UserProfileDto?> GetProfileDtoByOwnerId(long ownerId);
    Task<UserProfile?> GetProfileByOwnerId(long ownerId);
    public Task<bool> SaveAll();
}