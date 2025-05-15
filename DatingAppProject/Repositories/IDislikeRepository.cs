using DatingAppProject.DTO;
using DatingAppProject.Entities;

namespace DatingAppProject.Repositories;

public interface IDislikeRepository {
    Task DislikeUser(Dislike dislike);
    Task<bool> IsDisliked(long userId, long dislikedUserId);
    void RemoveDislike(Dislike dislike);
    Task<bool> SaveChangesAsync();
    Task<Dislike?> GetDislike(long sourceUserId, long targetUserId);
    Task<List<UserDto>> GetDislikedUsers(long userId);
}