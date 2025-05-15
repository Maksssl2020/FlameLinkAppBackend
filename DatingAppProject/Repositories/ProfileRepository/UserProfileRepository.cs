using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories.ProfileRepository;

public class UserProfileRepository(DataContext dataContext, IMapper mapper) : IUserProfileRepository{
    
    public async Task SaveProfile(AppUser registeredUser, string lookingFor){
        var userProfile = mapper.Map<UserProfile>(registeredUser);
        userProfile.LookingFor = lookingFor;
        
        registeredUser.UserProfile = userProfile;
        dataContext.Users.Update(registeredUser);

        await dataContext.UserProfiles.AddAsync(userProfile);
    }

    public async Task<UserProfileDto?> GetProfileDtoByOwnerId(long ownerId) {
        var interests = await dataContext.Interests
            .Where(i => i.Users.Any(u => u.Id == ownerId))
            .ProjectTo<InterestDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        var userProfile = await dataContext.UserProfiles
            .Include(up => up.Photos)
            .Include(up => up.MainPhoto)
            .Include(up => up.ProfileOwner)
            .Where(up => up.ProfileOwnerId == ownerId)
            .ProjectTo<UserProfileDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        
        if (userProfile != null) {
            userProfile.Interests = interests;
        }
        
        return userProfile;
    }

    public async Task<UserProfile?> GetProfileByOwnerId(long ownerId){
        return await dataContext.UserProfiles
            .Include(up => up.Photos)
            .Include(up => up.MainPhoto)
            .Include(up => up.ProfileOwner)
            .Where(up => up.ProfileOwnerId == ownerId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> SaveAll(){
        return await dataContext.SaveChangesAsync() > 0;
    }
}