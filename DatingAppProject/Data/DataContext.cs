using DatingAppProject.Entities;
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
    public DbSet<Image> Images { get; set; }
    public DbSet<ForumPost> ForumPosts { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserReview> UserReviews { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Dislike> Dislikes { get; set; }


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
        
        builder.Entity<UserProfile>()
            .HasOne(p => p.ProfileOwner)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfile>(p => p.ProfileOwnerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<UserProfile>()
            .HasOne(p => p.MainPhoto)
            .WithMany()
            .HasForeignKey(p => p.MainPhotoId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<ForumPostLike>()
            .HasKey(fpl => new { fpl.UserId, fpl.PostId });

        builder.Entity<ForumPostLike>()
            .HasOne(fpl => fpl.User)
            .WithMany()
            .HasForeignKey(fpl => fpl.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ForumPostLike>()
            .HasOne(fpl => fpl.Post)
            .WithMany(p => p.LikesList)
            .HasForeignKey(fpl => fpl.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<UserReview>()
            .HasOne(ur => ur.Reviewer)
            .WithMany()
            .HasForeignKey(ur => ur.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserReview>()
            .HasOne(ur => ur.ReviewedUser)
            .WithMany()
            .HasForeignKey(ur => ur.ReviewedUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Dislike>()
            .HasKey(d => new { d.SourceUserId, d.TargetUserId });

        builder.Entity<Dislike>()
            .HasOne(d => d.SourceUser)
            .WithMany()
            .HasForeignKey(d => d.SourceUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Dislike>()
            .HasOne(d => d.TargetUser)
            .WithMany()
            .HasForeignKey(d => d.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Match>()
            .HasOne(m => m.SourceUser)
            .WithMany()
            .HasForeignKey(m => m.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Match>()
            .HasOne(m => m.TargetUser)
            .WithMany()
            .HasForeignKey(m => m.TargetUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}