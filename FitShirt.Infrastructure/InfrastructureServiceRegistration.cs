using FitShirt.Domain.Designing.Repositories;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Domain.Purchasing.Repositories;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Repositories;
using FitShirt.Infrastructure.Designing.Persistence;
using FitShirt.Infrastructure.Publishing.Persistence;
using FitShirt.Infrastructure.Purchasing.Persistence;
using FitShirt.Infrastructure.Security.Persistence;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
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
        services.AddScoped<IDesignRepository, DesignRepository>();
        services.AddScoped<IShieldRepository, ShieldRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        
        return services;
    }
}