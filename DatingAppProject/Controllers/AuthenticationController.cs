using DatingAppProject.DTO;
using DatingAppProject.Exceptions;
using DatingAppProject.Repositories.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class AuthenticationController(IAuthenticationRepository authenticationRepository) : ControllerBase {

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationDto>> Register(RegisterRequestDto registerRequest){
        try {
            var registrationResult = await authenticationRepository.Register(registerRequest);
            return Ok(registrationResult);
        }
        catch (AuthenticationException exception) {
            return BadRequest(new { message = exception.Message });
        }
        catch (Exception exception) {
            return StatusCode(500, new { message = "An unexpected error has occurred. Please try again later." });
        }
    }

    [HttpPost("register-admin")]
    public async Task<ActionResult> RegisterAdmin(RegisterAdminRequestDto adminRequest){
        await authenticationRepository.RegisterAdmin(adminRequest);
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationDto>> Login(LoginRequestDto loginRequest){
        try {
            var loginResult = await authenticationRepository.Login(loginRequest);
            return Ok(loginResult);
        }
        catch (AuthenticationException authenticationException) {
            return BadRequest(new { message = authenticationException.Message });
        }
        catch (Exception exception) {
            return StatusCode(500, new { message = "An unexpected error has occurred. Please try again later." });
        }
    }
}