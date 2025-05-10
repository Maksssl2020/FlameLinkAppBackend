using DatingAppProject.DTO;

namespace DatingAppProject.Repositories;

public interface IUserReviewRepository {
    Task SaveReview(UserReviewRequestDto userReviewRequest);
    Task<bool> DeleteReview(long reviewId);
    Task<bool> UpdateReview(long reviewId, UserReviewUpdateRequestDto userReviewRequest);
    Task<List<UserReviewDto>> GetUserReviewsByUserId(long userId);
    Task<List<UserReviewDto>> GetCreatedReviewsByUserId(long userId);
    Task<List<UserReviewDto>> GetAllReviews();
    Task<bool> SaveChangesAsync();
}