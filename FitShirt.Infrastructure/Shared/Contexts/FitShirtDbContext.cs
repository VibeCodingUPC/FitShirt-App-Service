using FitShirt.Domain.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Shared.Contexts;

public class FitShirtDbContext : DbContext
{
    public FitShirtDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastUpdatedAt = DateTime.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}