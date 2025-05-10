namespace DatingAppProject.Entities;

public class Dislike {
    public long SourceUserId { get; set; }
    public AppUser SourceUser { get; set; } = null!;

    public long TargetUserId { get; set; }
    public AppUser TargetUser { get; set; } = null!;

    public DateTime DislikedAt { get; set; } = DateTime.UtcNow;
}