using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Models.Entities;
using FitShirt.Domain.Messaging.Models.Aggregates;
using FitShirt.Domain.OrderManagement.Models.Aggregates;
using FitShirt.Domain.OrderManagement.Models.ValueObjects;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Models.Entities;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Models.ValueObjects;
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
                    entry.Entity.IsEnable = true;
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

        builder.Entity<User>()
            .HasDiscriminator<int>("RoleId")
            .HasValue<Admin>((int)UserRoles.ADMIN)
            .HasValue<Client>((int)UserRoles.CLIENT)
            .HasValue<Seller>((int)UserRoles.SELLER);

        builder.Entity<User>()
            .HasOne(u => u.Role) // Un usuario tiene un rol
            .WithMany(r => r.Users) // Un rol puede tener muchos usuarios
            .HasForeignKey(u => u.RoleId) // La clave for�nea en User
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Role>()
            .Property(r => r.Name)
            .HasConversion(x => x.ToString(),
            x => (UserRoles) Enum.Parse(typeof(UserRoles), x));

        //-------

        builder.Entity<Post>()
            .HasOne(post => post.Category)
            .WithMany(category => category.Posts)
            .HasForeignKey(post => post.CategoryId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Post>()
            .HasOne(post => post.Color)
            .WithMany(color => color.Posts)
            .HasForeignKey(post => post.ColorId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Post>()
            .HasMany(post => post.PostSizes)
            .WithOne(ps => ps.Post)
            .HasForeignKey(ps => ps.PostId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Post>()
            .HasOne(p => p.PostPhoto)
            .WithOne(p => p.Post)
            .HasForeignKey<PostPhoto>(p => p.PostId)
            .IsRequired();

        // ------

        builder.Entity<Post>()
            .HasOne(post => post.User)
            .WithMany()
            .HasForeignKey(post => post.UserId);

        builder.Entity<Purchase>()
            .HasOne(purchase => purchase.User)
            .WithMany()
            .HasForeignKey(purchase => purchase.UserId);


        builder.Entity<Design>()
            .HasOne(design => design.User)
            .WithMany()
            .HasForeignKey(design => design.UserId);

        builder.Entity<Design>()
            .HasOne(design => design.PrimaryColor)
            .WithMany(color => color.DesignsPrimaryColor)
            .HasForeignKey(design => design.PrimaryColorId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Design>()
            .HasOne(design => design.SecondaryColor)
            .WithMany(color => color.DesignsSecondaryColor)
            .HasForeignKey(design => design.SecondaryColorId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Design>()
            .HasOne(design => design.TertiaryColor)
            .WithMany(color => color.DesignsTertiaryColor)
            .HasForeignKey(design => design.TertiaryColorId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Design>()
            .HasOne(design => design.Shield)
            .WithMany(shield => shield.Designs)
            .HasForeignKey(design => design.ShieldId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
        
        //---Message
        builder.Entity<Message>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(m => m.SentAt)
                .IsRequired();

            entity.HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        
        //----
        builder.Entity<DesignOrder>()
            .Property(d => d.Status)
            .HasConversion(x => x.ToString(), 
                x => (OrderStatus) Enum.Parse(typeof(OrderStatus), x));

        builder.Entity<DesignOrder>()
            .HasOne(d => d.Design)
            .WithMany()
            .IsRequired();

        builder.Entity<DesignOrder>()
            .HasOne(d => d.User)
            .WithMany()
            .IsRequired();
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Item> Items { get; set; }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostPhoto> PostPhotos { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PostSize> PostSizes { get; set; }
    
    public DbSet<Design> Designs { get; set; }
    
    public DbSet<Message> Messages { get; set; }
    public DbSet<Shield> Shields { get; set; }
    
    public DbSet<Color> Colors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<DesignOrder> DesignOrders { get; set; }
}