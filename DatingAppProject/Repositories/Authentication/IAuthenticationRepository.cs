using DatingAppProject.DTO;

namespace DatingAppProject.Repositories.Authentication;

public interface IAuthenticationRepository {

    Task<AuthenticationDto> Login(LoginRequestDto loginRequest);
    Task<AuthenticationDto> Register(RegisterRequestDto registerRequest);
    Task RegisterAdmin(RegisterAdminRequestDto registerAdminRequest);
    Task<bool> IsFieldTaken(string field, string value);
}