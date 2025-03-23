using DatingAppProject.Data;
using DatingAppProject.Repositories.Authentication;
using DatingAppProject.Repositories.InterestRepository;
using DatingAppProject.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.extensions;

public static class ApplicationServiceExtensions {

    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration){
        services.AddControllers();
        services.AddDbContext<DataContext>(options => {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IInterestRepository, InterestRepository>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSignalR();
        
        return services;
    }
}