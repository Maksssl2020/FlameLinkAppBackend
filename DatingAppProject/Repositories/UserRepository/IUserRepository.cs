using DatingAppProject.DTO;
using DatingAppProject.Helpers;

namespace DatingAppProject.Repositories.UserRepository;

public interface IUserRepository {
    Task<UserDto?> GetUserById(long id);
    Task<PaginationList<UserDto>> GetAllMatchingUsers(UserParams userParams);
    Task<bool> SaveAllAsync();
}