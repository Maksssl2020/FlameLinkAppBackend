using System.Text.RegularExpressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using Microsoft.EntityFrameworkCore;
using Match = DatingAppProject.Entities.Match;

namespace DatingAppProject.Repositories;

public class MatchesRepository(DataContext dataContext, IMapper mapper) : IMatchesRepository{

    public async Task LikeUser(Match match){
        await dataContext.Matches.AddAsync(match);
    }

    public void RemoveMatch(Match match){
        dataContext.Remove(match);
    }

    public async Task<bool> IsMatch(long userId, long matchId){
        return await dataContext.Matches.AnyAsync(m =>
                   m.SourceUserId == userId && m.TargetUserId == matchId) &&
               await dataContext.Matches.AnyAsync(m =>
                   m.SourceUserId == matchId && m.TargetUserId == userId);
    }

    public async Task<List<UserDto>> GetMatches(long userId){ 
        var likedByUser = await dataContext.Matches
            .Where(m => m.SourceUserId == userId)
            .Select(m => m.TargetUserId)
            .ToListAsync();
            
        var likedUser = await dataContext.Matches
            .Where(m => m.TargetUserId == userId)
            .Select(m => m.SourceUserId)
            .ToListAsync();
            
        var mutualMatchIds = likedByUser.Intersect(likedUser).ToList();

        var matchedUsers = await dataContext.Users
            .Where(u => mutualMatchIds.Contains(u.Id))
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return matchedUsers;
    }

    public async Task<List<UserDto>> GetLikedUsers(long userId){
        var likedUsersIds = await dataContext.Matches
            .Where(m => m.SourceUserId == userId)
            .Select(m => m.TargetUserId)
            .ToListAsync();

        var likedUsersData = await dataContext.Users
            .Where(u => likedUsersIds.Contains(u.Id))
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        
        return likedUsersData;
    }

    public async Task<bool> SaveChangesAsync(){
        return await dataContext.SaveChangesAsync() > 0;
    }

    public Task<Match?> GetMatch(long sourceUserId, long targetUserId){
        return dataContext.Matches
            .Include(m => m.SourceUser)
            .Include(m => m.TargetUser)
            .FirstOrDefaultAsync(m =>
                (m.SourceUserId == sourceUserId && m.TargetUserId == targetUserId) ||
                (m.SourceUserId == targetUserId && m.TargetUserId == sourceUserId));
    }
}