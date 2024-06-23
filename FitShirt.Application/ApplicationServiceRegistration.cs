using FitShirt.Application.Designing.Features.CommandServices;
using FitShirt.Application.Designing.Features.QueryServices;
using FitShirt.Application.Publishing.Features.CommandServices;
using FitShirt.Application.Publishing.Features.QueryServices;
using FitShirt.Application.Purchasing.Features.CommandServices;
using FitShirt.Application.Purchasing.Features.QueryServices;
using FitShirt.Application.Shared.Mapping;
using FitShirt.Domain.Designing.Services;
using FitShirt.Domain.Publishing.Services;
using FitShirt.Domain.Purchasing.Services;
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
        services.AddScoped<IDesignCommandService, DesignCommandService>();
        services.AddScoped<ICategoryCommandService, CategoryCommandService>();
        services.AddScoped<IPostQueryService, PostQueryService>();
        services.AddScoped<IDesignQueryService, DesignQueryService>();
        services.AddScoped<IShieldQueryService, ShieldQueryService>();
        services.AddScoped<ICategoryQueryService, CategoryQueryService>();
        services.AddScoped<IPurchaseCommandService, PurchaseCommandService>();
        services.AddScoped<IPurchaseQueryService, PurchaseQueryService>();
        
        return services;
    }
}