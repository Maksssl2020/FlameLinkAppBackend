using DatingAppProject.DTO;
using DatingAppProject.Entities;

namespace DatingAppProject.Repositories;

public interface IMatchesRepository {
    Task LikeUser(Match match);
    void RemoveMatch(Match match);
    Task<bool> IsMatch(long userId, long matchId);
    Task<List<UserDto>> GetMatches(long userId);
    Task<List<UserDto>> GetLikedUsers(long userId);
    Task<bool> SaveChangesAsync();
    Task<Match?> GetMatch(long sourceUserId, long targetUserId);
}