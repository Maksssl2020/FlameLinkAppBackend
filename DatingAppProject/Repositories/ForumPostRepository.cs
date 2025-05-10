using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Repositories.ImageRepository;
using DatingAppProject.Repositories.ProfileRepository;
using Microsoft.EntityFrameworkCore;
using static System.Enum;

namespace DatingAppProject.Repositories;

public class ForumPostRepository(DataContext dataContext, IMapper mapper, IUserProfileRepository userProfileRepository) : IForumPostRepository {

    public async Task CreatePost(ForumPostRequestDto forumPostRequest){
        var foundUser = await dataContext.Users.FirstOrDefaultAsync(user => user.Id == forumPostRequest.UserId);
        
        if (foundUser == null) {
            throw new Exception("User not found!");
        }

        var foundUserProfile = await userProfileRepository.GetProfileByOwnerId(foundUser.ProfileId);
        TryParse(forumPostRequest.Category, out ForumPostCategory forumPostCategory);

        var forumPost = new ForumPost {
            Author = foundUser.FirstName + " " + foundUser.LastName,
            AuthorAvatar = foundUserProfile?.MainPhoto ?? null,
            Title = forumPostRequest.Title,
            Content = forumPostRequest.Content,
            CreatedAt = DateTime.UtcNow,
            AuthorId = forumPostRequest.UserId, 
            AuthorUser = foundUser,
            Category = forumPostCategory,
        };
        
        await dataContext.ForumPosts.AddAsync(forumPost);
    }

    public async Task<ForumPostDto?> GetPostById(long postId){
        return await dataContext.ForumPosts
            .ProjectTo<ForumPostDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(post => post.Id == postId);
    }

    public async Task<List<ForumPostDto>> GetPostsByUserId(long userId){
        return await dataContext.ForumPosts 
            .Where(post => post.AuthorId == userId)
            .ProjectTo<ForumPostDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<List<ForumPostDto>> GetAllPosts(){
        return await dataContext.ForumPosts
            .ProjectTo<ForumPostDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<bool> DeletePost(long postId){
        var foundForumPost = await dataContext.ForumPosts.FirstOrDefaultAsync(post => post.Id == postId);
        if (foundForumPost == null) {
            throw new Exception("Post not found!");
        }
        
        dataContext.ForumPosts.Remove(foundForumPost);
        return await dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdatePost(long postId, ForumPostRequestDto forumPostRequest){
        var foundForumPost = await dataContext.ForumPosts.FirstOrDefaultAsync(post => post.Id == postId);
        if (foundForumPost == null) {
            throw new Exception("Post not found!");
        }

        foundForumPost.Title = forumPostRequest.Title;
        foundForumPost.Content = forumPostRequest.Content;
        foundForumPost.CreatedAt = DateTime.UtcNow;

        dataContext.ForumPosts.Update(foundForumPost);
        return await dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> SaveAllChanges(){
        return await dataContext.SaveChangesAsync() > 0;
    }
}