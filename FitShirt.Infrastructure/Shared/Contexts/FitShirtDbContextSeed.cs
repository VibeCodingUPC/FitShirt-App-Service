using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Models.Entities;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FitShirt.Infrastructure.Shared.Contexts;

public static class FitShirtDbContextSeed
{
    private static async Task<List<T>> LoadJsonAsync<T>(string relativePath)
    {
        var path = Path.Combine(AppContext.BaseDirectory, relativePath);

        if (!File.Exists(path))
            throw new FileNotFoundException($"Archivo no encontrado: {path}");

        var json = await File.ReadAllTextAsync(path);
        var list = JsonConvert.DeserializeObject<List<T>>(json);

        if (list == null)
            throw new Exception($"No se pudo deserializar el archivo: {path}");

        return list;
    }

    private static async Task LoadRolesDataAsync(FitShirtDbContext context)
    {
        if (!context.Roles.Any())
        {
            var roles = Enum.GetValues(typeof(UserRoles))
                .Cast<UserRoles>()
                .Select(role => new Role(role))
                .ToList();

            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
        }
    }

    private static async Task LoadCategoriesDataAsync(FitShirtDbContext context)
    {
        if (!context.Categories.Any())
        {
            var categories = await LoadJsonAsync<Category>("Shared/Data/categories.json");
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }
    }

    private static async Task LoadColorsDataAsync(FitShirtDbContext context)
    {
        if (!context.Colors.Any())
        {
            var colors = await LoadJsonAsync<Color>("Shared/Data/colors.json");
            context.Colors.AddRange(colors);
            await context.SaveChangesAsync();
        }
    }

    private static async Task LoadSizesDataAsync(FitShirtDbContext context)
    {
        if (!context.Sizes.Any())
        {
            var sizes = await LoadJsonAsync<Size>("Shared/Data/sizes.json");
            context.Sizes.AddRange(sizes);
            await context.SaveChangesAsync();
        }
    }

    private static async Task LoadShieldsDataAsync(FitShirtDbContext context)
    {
        if (!context.Shields.Any())
        {
            var shields = await LoadJsonAsync<Shield>("Shared/Data/shields.json");
            context.Shields.AddRange(shields);
            await context.SaveChangesAsync();
        }
    }

    private static async Task LoadUsersDataAsync(FitShirtDbContext context)
    {
        if (!context.Users.Any())
        {
            var adminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == UserRoles.ADMIN);

            if (adminRole == null)
                throw new Exception("Rol ADMIN no encontrado para crear el usuario");

            var user = new Admin
            {
                Name = "Diego",
                Lastname = "Defilippi",
                Username = "Diego_DefSan",
                Password = "$2a$10$p3LBQP5dB7T.67zXLliMPO55Er5.EX1TsizV3fLBXCF4hxB2wGdUq",
                Email = "ddefsan@test.com",
                Cellphone = "999999999",
                RoleId = adminRole.Id
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadDataAsync(FitShirtDbContext context)
    {
        try
        {
            await LoadRolesDataAsync(context);
            await LoadCategoriesDataAsync(context);
            await LoadColorsDataAsync(context);
            await LoadSizesDataAsync(context);
            await LoadShieldsDataAsync(context);
            await LoadUsersDataAsync(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error durante carga de datos iniciales: {ex.Message}");
            throw;
        }
    }
}
