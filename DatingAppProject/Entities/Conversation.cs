namespace DatingAppProject.Entities;

public class Conversation {
    public long Id { get; set; }
    
    public long RecipientId { get; set; }
    public AppUser Recipient { get; set; } = null!;

    public long SenderId { get; set; }
    public AppUser Sender { get; set; } = null!;
    
    public string LastMessage { get; set; } = string.Empty;
}