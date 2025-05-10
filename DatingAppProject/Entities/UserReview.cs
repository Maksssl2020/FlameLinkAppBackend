namespace DatingAppProject.Entities;

public class UserReview {
    public long Id { get; set; }
    public long ReviewerId { get; set; }
    public AppUser Reviewer { get; set; } = null!;

    public long ReviewedUserId { get; set; }
    public AppUser ReviewedUser { get; set; } = null!;

    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}