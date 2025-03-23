using DatingAppProject.Entities;
using Microsoft.AspNetCore.Identity;

namespace DatingAppProject.Data;

public static class AppRoleSeed {

    public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager){
        var roles = new List<AppRole> {
            new() {Name = "User"},
            new() {Name = "Admin"},
        };
        
        foreach (var role in roles) {
            if (role.Name != null && !await roleManager.RoleExistsAsync(role.Name)) {
                await roleManager.CreateAsync(role);
            }
        }
    }
}