using Microsoft.AspNetCore.Identity;

namespace DatingAppProject.Entities;

public class AppUserRole : IdentityUserRole<long> {
    public override long UserId { get; set; }
    public AppUser User { get; set; } = null!;
    
    public override long RoleId { get; set; }
    public AppRole Role { get; set; } = null!;
}