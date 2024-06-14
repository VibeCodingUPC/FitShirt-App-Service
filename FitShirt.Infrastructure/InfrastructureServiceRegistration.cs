using FitShirt.Infrastructure.Shared.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitShirt.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FitShirtConnection");

        services.AddDbContext<FitShirtDbContext>(options =>
        {
            options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString)
            );
        });

        // Add repositories injection
        // services.AddScoped<IReservationRepository, ReservationRepository>();
        
        return services;
    }
}