using Microsoft.AspNetCore.Identity;

namespace DatingAppProject.Entities;

public class AppRole : IdentityRole<long> {
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}