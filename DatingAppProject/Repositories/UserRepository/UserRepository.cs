using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories.UserRepository;

public class UserRepository(DataContext dataContext, IMapper mapper) : IUserRepository {
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
        
        // if (!string.IsNullOrEmpty(currentUser.Preference)) {
        //     users = users.Where(user => user.Gender == currentUser.Preference);
        // }

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
        
        users = users.Where(user => user.DateOfBirth >= minDob && user.DateOfBirth <= maxDob);
        
        return await PaginationList<UserDto>.Create(
            users.ProjectTo<UserDto>(mapper.ConfigurationProvider),
            userParams.PageNumber, 
            userParams.PageSize
            );
    }

    public async Task<bool> SaveAllAsync(){
        return await dataContext.SaveChangesAsync() > 0;
    }
}