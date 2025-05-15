namespace DatingAppProject.DTO;

public class MessageDto {
    public long Id { get; set; }
    public long SenderId { get; set; }
    public string SenderUsername { get; set; } = string.Empty;
    public ImageDto SenderAvatar { get; set; } = null!;
    
    public long RecipientId { get; set; }
    public string RecipientUsername { get; set; } = string.Empty;
    public ImageDto RecipientAvatar { get; set; } = null!;
    
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}