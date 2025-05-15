using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/forum-posts")]
public class ForumPostsController(IForumPostRepository forumPostRepository, DataContext dataContext) : ControllerBase {
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ForumPostDto>>> GetAllPosts() {
        var posts = await forumPostRepository.GetAllPosts();
        return Ok(posts);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ForumPostDto>> GetPostById(long id) {
        var post = await forumPostRepository.GetPostById(id);
        if (post == null) {
            return NotFound("Post not found.");
        }
        return Ok(post);
    }
    
    [HttpGet("post-likes/is-liked/{postId:long}/{userId:long}")]
    public async Task<ActionResult<bool>> IsPostLikedByUser(long postId, long userId) {
       var isLiked = await forumPostRepository.IsPostLikedByUser(postId, userId);
       return Ok(isLiked);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreatePost([FromBody] ForumPostRequestDto forumPostRequest) {
        await forumPostRepository.CreatePost(forumPostRequest);
        if (!await forumPostRepository.SaveAllChanges()) {
            return BadRequest("Failed to create post.");
        }
        
        return NoContent();
    }
    
    [HttpPost("post-likes/{postId:long}/{userId:long}")]
    public async Task<ActionResult> LikeOrUnlikePost(long postId, long userId) {
        var post = await dataContext.ForumPosts
            .Include(p => p.LikesList)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null) {
            return NotFound("Post not found.");
        }
        
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) {
            return NotFound("User not found.");
        }
        
        await forumPostRepository.LikePost(post, user);
        
        if (!await forumPostRepository.SaveAllChanges()) {
            return BadRequest("Failed to like post.");
        }
        
        return NoContent();
    }
    
    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdatePost(long id, [FromBody] ForumPostRequestDto forumPostRequest) {
        var post = await forumPostRepository.GetPostById(id);
        if (post == null) {
            return NotFound("Post not found.");
        }
        
        await forumPostRepository.UpdatePost(id, forumPostRequest);
        if (!await forumPostRepository.SaveAllChanges()) {
            return BadRequest("Failed to update post.");
        }
        
        return NoContent();
    }
    
    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeletePost(long id) {
        var post = await forumPostRepository.GetPostById(id);
        if (post == null) {
            return NotFound("Post not found.");
        }
        
        await forumPostRepository.DeletePost(id);
        if (!await forumPostRepository.SaveAllChanges()) {
            return BadRequest("Failed to delete post.");
        }
        
        return NoContent();
    }
}