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
        if (await authenticationRepository.IsFieldTaken("username", registerRequest.Username)) {
            return BadRequest("Username already taken.");
        }
        
        if (await authenticationRepository.IsFieldTaken("email", registerRequest.Email)) {
            return BadRequest("Email already taken.");
        }
        
        try {
            var registrationResult = await authenticationRepository.Register(registerRequest);
            return Ok(registrationResult);
        }
        catch (Exception exception) {
            if (exception is CustomAuthenticationException authenticationException) {
                return BadRequest(new ApiException(BadRequest().StatusCode, authenticationException.Message, exception.StackTrace));
            }
            
            return BadRequest(new ApiException(BadRequest().StatusCode, "Something went wrong. Please try again later.", exception.Message));
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
        catch (Exception exception) {
            return BadRequest(new ApiException(BadRequest().StatusCode, exception.Message, exception.StackTrace));
        }
    }
}