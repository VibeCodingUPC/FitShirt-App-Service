using FitShirt.Application.Publishing.Features.CommandServices;
using FitShirt.Application.Publishing.Features.QueryServices;
using FitShirt.Application.Shared.Mapping;
using FitShirt.Domain.Publishing.Services;
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
        services.AddScoped<IPostCommandService, PostCommandService>();
        services.AddScoped<ICategoryCommandService, CategoryCommandService>();
        services.AddScoped<IPostQueryService, PostQueryService>();
        services.AddScoped<ICategoryQueryService, CategoryQueryService>();
        
        return services;
    }
}