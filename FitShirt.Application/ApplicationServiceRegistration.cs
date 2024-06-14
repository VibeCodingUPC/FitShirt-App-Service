using FitShirt.Application.Shared.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace FitShirt.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(RequestToModel),
            typeof(ModelToResponse)
        );
        
        // Add services injection
        // services.AddScoped<IReservationCommandService, ReservationCommandServiceService>();
        
        return services;
    }
}