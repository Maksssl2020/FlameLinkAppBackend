using Microsoft.AspNetCore.Identity;

namespace DatingAppProject.Entities.User;

public class AppUser : IdentityUser<long> {
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string Preference { get; set; }
    public required string Interests { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}