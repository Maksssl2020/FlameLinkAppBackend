using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories;

public class UserReviewRepository(DataContext dataContext, IMapper mapper) : IUserReviewRepository {

    public async Task SaveReview(UserReviewRequestDto userReviewRequest){
        var foundReviewer = await dataContext.Users.FirstOrDefaultAsync(user => user.Id == userReviewRequest.ReviewerId);
        var foundReviewedUser = await dataContext.Users.FirstOrDefaultAsync(user => user.Id == userReviewRequest.ReviewedUserId);

        if (foundReviewedUser == null || foundReviewer == null) {
            throw new Exception("User not found!");
        }

        var userReview = new UserReview {
            ReviewedUserId = userReviewRequest.ReviewedUserId,
            ReviewedUser = foundReviewedUser,
            ReviewerId = userReviewRequest.ReviewerId,
            Reviewer = foundReviewer,
            Content = userReviewRequest.Content,
            Rating = userReviewRequest.Rating,
            CreatedAt = DateTime.UtcNow,
        };
        
        await dataContext.UserReviews.AddAsync(userReview);
    }

    public async Task<bool> DeleteReview(long reviewId){
        var review = await dataContext.UserReviews
            .Include(r => r.Reviewer)
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        if (review == null) {
            return false;
        }

        dataContext.UserReviews.Remove(review);
        return await dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateReview(long reviewId, UserReviewUpdateRequestDto userReviewRequest){
        var review = await dataContext.UserReviews
            .Include(r => r.Reviewer)
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        if (review == null) {
            return false;
        }

        review.Content = userReviewRequest.Content;
        review.Rating = userReviewRequest.Rating;

        dataContext.UserReviews.Update(review);
        return await dataContext.SaveChangesAsync() > 0;
    }

    public async Task<List<UserReviewDto>> GetUserReviewsByUserId(long userId){
        return await dataContext.UserReviews
            .Where(review => review.ReviewedUserId == userId)
            .ProjectTo<UserReviewDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<List<UserReviewDto>> GetCreatedReviewsByUserId(long userId){
        return await dataContext.UserReviews
            .Where(review => review.ReviewerId == userId)
            .ProjectTo<UserReviewDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<List<UserReviewDto>> GetAllReviews(){
        return await dataContext.UserReviews
            .ProjectTo<UserReviewDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<bool> SaveChangesAsync(){
        return await dataContext.SaveChangesAsync() > 0;
    }
}