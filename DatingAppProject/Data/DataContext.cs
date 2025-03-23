using DatingAppProject.Entities;
using DatingAppProject.Entities.InterestEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<
    AppUser,AppRole, long, IdentityUserClaim<long>,
    AppUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>,
    IdentityUserToken<long>
>(options) {
    public DbSet<Interest> Interests { get; set; }

    protected override void OnModelCreating(ModelBuilder builder){
        base.OnModelCreating(builder);
        
        builder.Entity<AppUserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        builder.Entity<AppUserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<AppUserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Interest>()
            .HasIndex(i => i.InterestName).IsUnique();
    }
}