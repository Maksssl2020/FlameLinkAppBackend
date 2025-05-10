namespace DatingAppProject.Entities;

public class Message {
    public long Id { get; set; }
    public long SenderId { get; set; }
    public AppUser Sender { get; set; } = null!;

    public long RecipientId { get; set; }
    public AppUser Recipient { get; set; } = null!;

    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}