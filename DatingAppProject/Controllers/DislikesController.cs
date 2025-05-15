using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/dislikes")]
public class DislikesController(IDislikeRepository dislikeRepository, UserManager<AppUser> userManager) : ControllerBase {

    [HttpGet("{sourceUserId:long}/disliked-users")]
    public async Task<ActionResult<List<UserDto>>> GetDislikedUsers([FromRoute] long sourceUserId) {
        var dislikedUsers = await dislikeRepository.GetDislikedUsers(sourceUserId);
        return Ok(dislikedUsers);
    }
    
    [HttpGet("{sourceUserId:long}/is-disliked/{dislikedUserId:long}")]
    public async Task<ActionResult<bool>> IsDisliked([FromRoute] long sourceUserId, [FromRoute] long dislikedUserId){
        var isDisliked = await dislikeRepository.IsDisliked(sourceUserId, dislikedUserId);
        return Ok(isDisliked);
    }
    
    [HttpPost("{sourceUserId:long}/dislike/{dislikedUserId:long}")]
    public async Task<ActionResult> DislikeUser([FromRoute] long sourceUserId, [FromRoute] long dislikedUserId) {
        var foundSourceUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == sourceUserId);
        var foundDislikedUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == dislikedUserId);

        if (foundSourceUser == null || foundDislikedUser == null) {
            return NotFound("User not found.");
        }

        var dislike = new Dislike {
            SourceUser = foundSourceUser,
            SourceUserId = foundSourceUser.Id,
            TargetUser = foundDislikedUser,
            TargetUserId = foundDislikedUser.Id
        };
        
        await dislikeRepository.DislikeUser(dislike);
        if (!await dislikeRepository.SaveChangesAsync()) {
            return BadRequest("Failed to save dislike.");
        }

        return Ok();
    }
    
    [HttpDelete("{sourceUserId:long}/remove-dislike/{dislikedUserId:long}")]
    public async Task<ActionResult> RemoveDislike([FromRoute] long sourceUserId, [FromRoute] long dislikedUserId){
        var foundDislike = await dislikeRepository.GetDislike(sourceUserId, dislikedUserId);
        
        if (foundDislike == null) {
            return NotFound("Dislike not found.");
        }
        
        dislikeRepository.RemoveDislike(foundDislike);

        if (!await dislikeRepository.SaveChangesAsync()) {
            return BadRequest("Failed to remove dislike.");
        }
        
        return Ok("Dislike removed.");
    }
}