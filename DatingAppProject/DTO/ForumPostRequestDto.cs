namespace DatingAppProject.DTO;

public class ForumPostRequestDto {
    public long UserId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Category { get; set; }
}