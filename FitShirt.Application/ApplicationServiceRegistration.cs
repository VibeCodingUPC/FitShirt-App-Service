using FitShirt.Application.Designing.Features.CommandServices;
using FitShirt.Application.Designing.Features.QueryServices;
using FitShirt.Application.Publishing.Features.CommandServices;
using FitShirt.Application.Publishing.Features.QueryServices;
using FitShirt.Application.Purchasing.Features.CommandServices;
using FitShirt.Application.Purchasing.Features.QueryServices;
using FitShirt.Application.Security.Features.CommandServices;
using FitShirt.Application.Security.Features.OutboundServices;
using FitShirt.Application.Security.Features.QueryServices;
using FitShirt.Application.Shared.Features.QueryServices;
using FitShirt.Application.Shared.Mapping;
using FitShirt.Domain.Designing.Services;
using FitShirt.Domain.Publishing.Services;
using FitShirt.Domain.Purchasing.Services;
using FitShirt.Domain.Security.Services;
using FitShirt.Domain.Shared.Services;
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
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<IPostQueryService, PostQueryService>();
        services.AddScoped<IDesignQueryService, DesignQueryService>();
        services.AddScoped<IShieldQueryService, ShieldQueryService>();
        services.AddScoped<ICategoryQueryService, CategoryQueryService>();
        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IPurchaseCommandService, PurchaseCommandService>();
        services.AddScoped<IPurchaseQueryService, PurchaseQueryService>();
        services.AddScoped<IColorQueryService, ColorQueryService>();
        services.AddScoped<ISizeQueryService, SizeQueryService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEncryptService, EncryptService>();
        services.AddScoped<IGoogleCaptchaValidator, GoogleCaptchaValidator>();
        services.AddHttpClient(); 
        return services;
    }
}