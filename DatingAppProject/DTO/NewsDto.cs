namespace DatingAppProject.DTO;

public class NewsDto {
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Content { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required ImageDto CoverImage { get; set; }
}