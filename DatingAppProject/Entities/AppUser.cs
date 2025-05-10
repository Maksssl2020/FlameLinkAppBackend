using Microsoft.AspNetCore.Identity;

namespace DatingAppProject.Entities;

public class AppUser : IdentityUser<long> {
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string Preference { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;

    public long ProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
    
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
    public List<Interest> Interests { get; set; } = [];
}