using DatingAppProject.Entities.ImageEntity;

namespace DatingAppProject.Entities.NewsEntity;

public class News {
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Content { get; set; }
    public required long CoverImageId { get; set; }
    public Image CoverImage { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}