using DatingAppProject.DTO;
using DatingAppProject.Entities;

namespace DatingAppProject.Repositories;

public interface IForumPostRepository {
    Task CreatePost(ForumPostRequestDto forumPostRequest);
    Task<ForumPostDto?> GetPostById(long postId);
    Task<List<ForumPostDto>> GetPostsByUserId(long userId);
    Task<List<ForumPostDto>> GetAllPosts();
    Task<bool> IsPostLikedByUser(long postId, long userId);
    Task<bool> DeletePost(long postId);
    Task<bool> UpdatePost(long postId, ForumPostRequestDto forumPostRequest);
    Task LikePost(ForumPost forumPost, AppUser user);
    Task<bool> SaveAllChanges();    
}