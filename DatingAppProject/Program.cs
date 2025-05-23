using DatingAppProject.Data;
using DatingAppProject.Entities;
using DatingAppProject.extensions;
using DatingAppProject.Helpers;
using DatingAppProject.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject;

public class Program {
    public static async Task Main(string[] args){
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration);

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseCors(x => 
            x.AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:5173", "http://localhost:5174")
                .AllowCredentials()
        );
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        
        app.MapControllers();
        
        await using var scope = app.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;
        
        try {
            var context = services.GetRequiredService<DataContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            await context.Database.MigrateAsync();
            await AppRoleSeed.SeedRolesAsync(roleManager);
            // await SeedHelper.SeedInterestsAsync(context);
            // await SeedHelper.SeedUsersAsync(userManager, roleManager,  context);
        }
        catch (Exception e) {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(e, "An error occurred while migrating the database.");
        }
        
        app.Run();
    }
}