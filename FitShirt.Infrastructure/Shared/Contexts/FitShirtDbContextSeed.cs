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
    private static async Task LoadRolesDataAsync(FitShirtDbContext context)
    {
        try
        {
            if (!context.Roles.Any())
            {
                var roles = Enum.GetValues(typeof(UserRoles))
                    .Cast<UserRoles>()
                    .Select(role => new Role(role))
                    .ToList();

                context.Roles.AddRange(roles!);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving roles in database");
        }
    }
    
    private static async Task LoadCategoriesDataAsync(FitShirtDbContext context)
    {
        try
        {
            if (!context.Categories.Any())
            {
                var categoriesData = File.ReadAllText("../FitShirt.Infrastructure/Shared/Data/categories.json");

                var categories = JsonConvert.DeserializeObject<List<Category>>(categoriesData);

                context.Categories.AddRange(categories!);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving categories in database");
        }
    }
    
    private static async Task LoadColorsDataAsync(FitShirtDbContext context)
    {
        try
        {
            if (!context.Colors.Any())
            {
                var colorsData = File.ReadAllText("../FitShirt.Infrastructure/Shared/Data/colors.json");

                var colors = JsonConvert.DeserializeObject<List<Color>>(colorsData);

                context.Colors.AddRange(colors!);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving colors in database");
        }
    }
    
    private static async Task LoadSizesDataAsync(FitShirtDbContext context)
    {
        try
        {
            if (!context.Sizes.Any())
            {
                var sizesData = await File.ReadAllTextAsync("../FitShirt.Infrastructure/Shared/Data/sizes.json");

                var sizes = JsonConvert.DeserializeObject<List<Size>>(sizesData);

                context.Sizes.AddRange(sizes!);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving sizes in database");
        }
    }
    
    private static async Task LoadShieldsDataAsync(FitShirtDbContext context)
    {
        try
        {
            if (!context.Shields.Any())
            {
                var shieldData = await File.ReadAllTextAsync("../FitShirt.Infrastructure/Shared/Data/shields.json");

                var shields = JsonConvert.DeserializeObject<List < Shield >> (shieldData);
                
                context.Shields.AddRange(shields!);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving shields in database");
        }
    }
    
    private static async Task LoadUsersDataAsync(FitShirtDbContext context)
    {
        try
        {
            if (!context.Users.Any())
            {
                var adminRole = await context.Roles
                    .FirstOrDefaultAsync(r => r.Name == UserRoles.ADMIN);

                var user = new Admin
                {
                    Name = "Diego",
                    Lastname = "Defilippi",
                    Username = "Diego_DefSan",
                    Password = "$2a$10$p3LBQP5dB7T.67zXLliMPO55Er5.EX1TsizV3fLBXCF4hxB2wGdUq",
                    Email = "ddefsan@test.com",
                    Cellphone = "999999999",
                    RoleId = adminRole!.Id
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving users in database");
        }
    }
    
    public static async Task LoadDataAsync(FitShirtDbContext context)
    {
        await LoadRolesDataAsync(context);
        await LoadCategoriesDataAsync(context);
        await LoadColorsDataAsync(context);
        await LoadSizesDataAsync(context);
        await LoadShieldsDataAsync(context);
        await LoadUsersDataAsync(context);
    }
}