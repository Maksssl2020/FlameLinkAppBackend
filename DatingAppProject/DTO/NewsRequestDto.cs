namespace DatingAppProject.DTO;

public class NewsRequestDto {
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Content { get; set; }
    public required IFormFile CoverImage { get; set; }
}