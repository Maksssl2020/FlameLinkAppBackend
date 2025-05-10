using DatingAppProject.Data;
using DatingAppProject.Repositories;
using DatingAppProject.Repositories.Authentication;
using DatingAppProject.Repositories.ImageRepository;
using DatingAppProject.Repositories.InterestRepository;
using DatingAppProject.Repositories.ProfileRepository;
using DatingAppProject.Repositories.UserRepository;
using DatingAppProject.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.extensions;

public static class ApplicationServiceExtensions {

    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration){
        services.AddControllers();
        services.AddDbContext<DataContext>(options => {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(120));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IInterestRepository, InterestRepository>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IForumPostRepository, ForumPostRepository>();
        services.AddScoped<IUserReviewRepository, UserReviewRepository>();
        services.AddScoped<IMatchesRepository, MatchesRepository>();
        services.AddScoped<IDislikeRepository, DislikeRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSignalR();
        
        return services;
    }
}