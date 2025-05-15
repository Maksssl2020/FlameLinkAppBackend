using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.extensions;
using DatingAppProject.Helpers;
using DatingAppProject.Repositories.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(IUserRepository userRepository, UserManager<AppUser> userManager): ControllerBase {

    [HttpGet("matching-users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetMatchingUsers([FromQuery] UserParams userParams){
        var matchingUsers = await userRepository.GetAllMatchingUsers(userParams);
        Response.AddPaginationHeader(matchingUsers);
        
        return Ok(matchingUsers);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<UserDto>> GetUserById(long id){
        var foundUser = await userRepository.GetUserById(id);

        if (foundUser == null) {
            return NotFound("User not found.");
        }
        
        return Ok(foundUser);
    }
    
    [Authorize]
    [HttpPatch("change-account-data")]
    public async Task<IActionResult> ChangeAccountData([FromForm] UpdateAccountDataRequest updateAccountDataRequest) {
        var user = await userManager.GetUserAsync(User);
        if (user == null) {
            return Unauthorized("User not found.");
        }

        if (updateAccountDataRequest.Email != null) {
            var isEmailTaken = await userRepository.IsEmailTaken(updateAccountDataRequest.Email);

            if (!isEmailTaken) {
                user.Email = updateAccountDataRequest.Email;
            }
            else {
                return BadRequest($"Email {updateAccountDataRequest.Email} is already taken.");
            }
        }
        
        if (updateAccountDataRequest.FirstName != null) {
            user.FirstName = updateAccountDataRequest.FirstName;
        }

        if (updateAccountDataRequest.LastName != null) {
            user.LastName = updateAccountDataRequest.LastName;
        }

        if (updateAccountDataRequest.City != null) {
            user.City = updateAccountDataRequest.City;
        }

        if (updateAccountDataRequest.Country != null) {
            user.Country = updateAccountDataRequest.Country;
        }

        await userRepository.SaveAllAsync();
        return Ok("User has been updated.");
    }
    
    [Authorize]
    [HttpPut("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequestDto){
        var user = await userManager.GetUserAsync(User);
        if (user == null) {
            return Unauthorized("User not found.");
        }

        var result = await userManager.ChangePasswordAsync(user, changePasswordRequestDto.CurrentPassword, changePasswordRequestDto.NewPassword);
        if (result.Succeeded) return Ok("Password changed.");
        
        var errors = result.Errors.Select(e => e.Description);
        return BadRequest(new {Errors = errors});
    }
}