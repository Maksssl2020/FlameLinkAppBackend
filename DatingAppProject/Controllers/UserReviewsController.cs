using DatingAppProject.DTO;
using DatingAppProject.Exceptions;
using DatingAppProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/user-reviews")]
public class UserReviewsController(IUserReviewRepository userReviewRepository) : ControllerBase {
    
    [HttpGet]
    public async Task<ActionResult<List<UserReviewDto>>> GetAllUserReviews() {
        var reviews = await userReviewRepository.GetAllReviews();
        return Ok(reviews);
    }
    
    [HttpGet("user-created-reviews/{userId}")]
    public async Task<ActionResult<List<UserReviewDto>>> GetAllUserReviews(long userId) {
        var reviews = await userReviewRepository.GetCreatedReviewsByUserId(userId);
        return Ok(reviews);
    }
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<UserReviewDto>>> GetUserReviews(long userId) {
        var reviews = await userReviewRepository.GetUserReviewsByUserId(userId);
        return Ok(reviews);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateUserReview(UserReviewRequestDto userReviewRequest) {
        try {
            await userReviewRepository.SaveReview(userReviewRequest);
            if (await userReviewRepository.SaveChangesAsync()) {
                return Ok("User review created.");
            }
            
            return BadRequest("Cannot create user review.");
        } catch (Exception exception) {
            return BadRequest(new ApiException(BadRequest().StatusCode, exception.Message, exception.StackTrace));
        }
    }
    
    [HttpDelete("{reviewId}")]
    public async Task<ActionResult> DeleteUserReview(long reviewId) {
        try {
            if (await userReviewRepository.DeleteReview(reviewId)) {
                return Ok("User review deleted.");
            }
            
            return BadRequest("Cannot delete user review.");
        } catch (Exception exception) {
            return BadRequest(new ApiException(BadRequest().StatusCode, exception.Message, exception.StackTrace));
        }
    }
    
    [HttpPut("{reviewId}")]
    public async Task<ActionResult> UpdateUserReview(long reviewId, UserReviewUpdateRequestDto userReviewUpdateRequest) {
        try {
            if (await userReviewRepository.UpdateReview(reviewId, userReviewUpdateRequest)) {
                return Ok("User review updated.");
            }
            
            return BadRequest("Cannot update user review.");
        } catch (Exception exception) {
            return BadRequest(new ApiException(BadRequest().StatusCode, exception.Message, exception.StackTrace));
        }
    }
}