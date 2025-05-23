using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories;

public class DislikeRepository(DataContext dataContext, IMapper mapper) : IDislikeRepository {

    public async Task DislikeUser(Dislike dislike){
        await dataContext.Dislikes.AddAsync(dislike);
    }

    public async Task<bool> IsDisliked(long userId, long dislikedUserId){
        return await dataContext.Dislikes
            .AnyAsync(d => d.SourceUserId == userId && d.TargetUserId == dislikedUserId);
    }

    public void RemoveDislike(Dislike dislike){
        dataContext.Dislikes.Remove(dislike);
    }

    public async Task<bool> SaveChangesAsync(){
        return await dataContext.SaveChangesAsync() > 0;
    }

    public async Task<Dislike?> GetDislike(long sourceUserId, long targetUserId){
        return await dataContext.Dislikes
            .FirstOrDefaultAsync(d => d.SourceUserId == sourceUserId && d.TargetUserId == targetUserId);
    }

    public Task<List<UserDto>> GetDislikedUsers(long userId){
        return dataContext.Dislikes
            .Where(d => d.SourceUserId == userId)
            .Select(d => d.TargetUser)
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
}