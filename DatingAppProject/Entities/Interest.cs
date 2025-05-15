namespace DatingAppProject.Entities;

public class Interest {

    public long Id { get; set; }
    public required string InterestName { get; set; }
    
    public ICollection<AppUser> Users { get; set; } = [];
}