using DatingAppProject.DTO;
using DatingAppProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/forum-posts")]
public class ForumPostsController(IForumPostRepository forumPostRepository) : ControllerBase {
    
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
    
    [HttpPost]
    public async Task<ActionResult> CreatePost([FromBody] ForumPostRequestDto forumPostRequest) {
        await forumPostRepository.CreatePost(forumPostRequest);
        if (!await forumPostRepository.SaveAllChanges()) {
            return BadRequest("Failed to create post.");
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