using System.Security.Claims;

namespace DatingAppProject.extensions;

public static class ClaimsPrincipleExtensions {

    public static string GetUsername(this ClaimsPrincipal user){
        var username = user.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("Cannot get username from token.");
        return username;
    }
    
    public static long GetUserId(this ClaimsPrincipal user){
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot get user ID from token."));
        return userId;
    }

    public static string GetRole(this ClaimsPrincipal user){
        var role = user.FindFirstValue(ClaimTypes.Role) ?? throw new Exception("Cannot get role from token.");
        return role;
    }
}