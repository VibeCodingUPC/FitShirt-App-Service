using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Purchasing.Models.Entities;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Publishing.Models.Aggregates;

public class Post : BaseModel
{
    public string Name { get; set; }
    public string Image { get; set; }
    public int Stock { get; set; }
    public double Price { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public int ColorId { get; set; }
    public Color Color { get; set; }
    
    public ICollection<PostSize> PostSizes { get; set; }
    public ICollection<Item> Items { get; set; } = new List<Item>();
    
    public int UserId { get; set; }
    public Seller User { get; set; }
    
}