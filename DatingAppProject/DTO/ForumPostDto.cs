using System.Text.Json.Serialization;
using DatingAppProject.Entities;

namespace DatingAppProject.DTO;

public class ForumPostDto {
    public long Id { get; set; }
    public long AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ForumPostCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public ImageDto AuthorImage { get; set; } = null!;
}