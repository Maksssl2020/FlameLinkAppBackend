namespace DatingAppProject.DTO;

public class ConversationPreviewDto {
    public long UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public ImageDto Avatar { get; set; } = null!;
    public string LastMessage { get; set; } = string.Empty;
    public DateTime LastMessageSentAt { get; set; }
}