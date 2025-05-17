using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Helpers;
using DatingAppProject.Repositories.InterestRepository;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories.UserRepository;

public class UserRepository(DataContext dataContext, IInterestRepository interestRepository, IMapper mapper) : IUserRepository {
    public async Task<UserDto?> GetUserById(long id){
        return await dataContext.Users
            .Where(user => user.Id == id)
            .Include(user => user.Interests)
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<PaginationList<UserDto>> GetAllMatchingUsers(UserParams userParams) {
        var currentUser = await dataContext.Users
            .FirstOrDefaultAsync(user => user.UserName == userParams.UserUsername);

        if (currentUser == null) {
            throw new Exception("User not found");
        }
        
        var likedUserIds = await dataContext.Matches
            .Where(m => m.SourceUserId == currentUser.Id)
            .Select(m => m.TargetUserId)
            .ToListAsync();

        var dislikedUserIds = await dataContext.Dislikes
            .Where(d => d.SourceUserId == currentUser.Id)
            .Select(d => d.TargetUserId)
            .ToListAsync();
        
        var users = dataContext.Users
            .Include(user => user.UserProfile.MainPhoto)
            .Where(user => user.UserName != userParams.UserUsername &&
                           !likedUserIds.Contains(user.Id) &&
                           !dislikedUserIds.Contains(user.Id))
            .AsQueryable();


        users = currentUser.Preference switch {
            "Males" => users.Where(user => user.Gender == "Male"),
            "Females" => users.Where(user => user.Gender == "Female"),
            _ => users
        };

        users = userParams.UserParamsImagesOptions switch {
            UserParamsImagesOptions.With => users.Where(u =>
                u.UserProfile.MainPhoto != null || u.UserProfile.Photos.Count > 0),
            UserParamsImagesOptions.Without => users.Where(u =>
                u.UserProfile.MainPhoto == null && u.UserProfile.Photos.Count == 0),
            _ => users
        };

        var foundInterests = await interestRepository.GetAllByNames(userParams.Interests);
        if (foundInterests.Count > 0) {
            var foundInterestNames = foundInterests.Select(i => i.InterestName).ToList();
            users = users.Where(u => u.Interests.Any(ui => foundInterestNames.Contains(ui.InterestName)));
        }

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
        
        users = users.Where(user => user.DateOfBirth >= minDob && user.DateOfBirth <= maxDob);
        
        return await PaginationList<UserDto>.Create(
            users.ProjectTo<UserDto>(mapper.ConfigurationProvider),
            userParams.PageNumber, 
            userParams.PageSize
            );
    }

    public async Task<bool> IsEmailTaken(string emailValue) {
        return await dataContext.Users.AnyAsync(user => user.Email == emailValue);
    }

    public async Task<bool> SaveAllAsync(){
        return await dataContext.SaveChangesAsync() > 0;
    }
}