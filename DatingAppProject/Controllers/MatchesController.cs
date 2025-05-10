using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/matches")]
public class MatchesController(IMatchesRepository matchesRepository, UserManager<AppUser> userManager) : ControllerBase {

    [HttpGet("{sourceUserId:long}/matches")]
    public async Task<ActionResult<List<UserDto>>> GetMatches([FromRoute] long sourceUserId) {
        var matches = await matchesRepository.GetMatches(sourceUserId);
        return Ok(matches);
    }
    
    [HttpGet("{sourceUserId:long}/liked-users")]
    public async Task<ActionResult<List<UserDto>>> GetLikedUsers([FromRoute] long sourceUserId) {
        var likedUsers = await matchesRepository.GetLikedUsers(sourceUserId);
        return Ok(likedUsers);
    }
    
    [HttpGet("{sourceUserId:long}/is-match/{targetUserId:long}")]
    public async Task<ActionResult<bool>> IsMatch([FromRoute] long sourceUserId, [FromRoute] long targetUserId){
        var isMatch = await matchesRepository.IsMatch(sourceUserId, targetUserId);
        return Ok(isMatch);
    }
    
    [HttpPost("{sourceUserId:long}/like/{targetUserId:long}")]
    public async Task<ActionResult> LikeUser([FromRoute] long sourceUserId, [FromRoute] long targetUserId) {
        var foundSourceUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == sourceUserId);
        var foundTargetUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == targetUserId);

        if (foundSourceUser == null || foundTargetUser == null) {
            return NotFound("User not found.");
        }

        var match = new Match {
            SourceUser = foundSourceUser,
            TargetUser = foundTargetUser,
            SourceUserId = sourceUserId,
            TargetUserId = targetUserId
        };

        await matchesRepository.LikeUser(match);
        if (await matchesRepository.SaveChangesAsync()) {
            return Ok("User liked successfully.");
        }
        
        return BadRequest("Failed to like user.");
    }
    
    [HttpDelete("{sourceUserId:long}/remove-like/{targetUserId:long}")]
    public async Task<ActionResult> RemoveLike([FromRoute] long sourceUserId, [FromRoute] long targetUserId){
        var match = await matchesRepository.GetMatch(sourceUserId, targetUserId);

        if (match == null) {
            return NotFound("The match does not exist.");
        }

        matchesRepository.RemoveMatch(match);
        
        if (await matchesRepository.SaveChangesAsync()) {
            return Ok("Like removed successfully.");
        }
        
        return BadRequest("Failed to remove like.");
    }
}