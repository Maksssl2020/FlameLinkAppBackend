namespace DatingAppProject.Entities;

public class ForumPostLike {
    public long UserId { get; set; }
    public AppUser User { get; set; } = null!;
    
    public long PostId { get; set; }
    public ForumPost Post { get; set; } = null!;
}