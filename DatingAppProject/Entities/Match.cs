namespace DatingAppProject.Entities;

public class Match {
    public long Id { get; set; }
    
    public AppUser SourceUser { get; set; } = null!;
    public long SourceUserId { get; set; }
    
    public AppUser TargetUser { get; set; } = null!;
    public long TargetUserId { get; set; }
}