namespace DatingAppProject.DTO;

public class UserReviewUpdateRequestDto {
    public required string Content { get; set; }
    public int Rating { get; set; }
}