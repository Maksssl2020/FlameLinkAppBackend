namespace DatingAppProject.DTO;

public class MessageRequestDto {
    public long SenderId { get; set; }
    public long RecipientId { get; set; }
    public required string Content { get; set; }
}