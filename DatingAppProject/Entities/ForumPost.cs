namespace DatingAppProject.Entities;

public class ForumPost {
    public long Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Image? AuthorAvatar { get; set; }
    public ForumPostCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Content { get; set; }
    
    public long AuthorId { get; set; }
    public AppUser AuthorUser { get; set; }
    public List<ForumPostLike> LikesList { get; set; } = [];
}