using DatingAppProject.DTO;

namespace DatingAppProject.Repositories;

public interface IForumPostRepository {
    Task CreatePost(ForumPostRequestDto forumPostRequest);
    Task<ForumPostDto?> GetPostById(long postId);
    Task<List<ForumPostDto>> GetPostsByUserId(long userId);
    Task<List<ForumPostDto>> GetAllPosts();
    Task<bool> DeletePost(long postId);
    Task<bool> UpdatePost(long postId, ForumPostRequestDto forumPostRequest);
    Task<bool> SaveAllChanges();    
}