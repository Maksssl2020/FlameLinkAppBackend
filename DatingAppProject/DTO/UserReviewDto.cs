namespace DatingAppProject.DTO;

public class UserReviewDto {
    public long Id { get; set; }
    public long ReviewerId { get; set; }
    public long ReviewedUserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}