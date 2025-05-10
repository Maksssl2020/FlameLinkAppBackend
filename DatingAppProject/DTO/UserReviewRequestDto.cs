namespace DatingAppProject.DTO;

public class UserReviewRequestDto {
    public long ReviewerId { get; set; }
    public long ReviewedUserId { get; set; }
    public required string Content { get; set; }
    public int Rating { get; set; }
}