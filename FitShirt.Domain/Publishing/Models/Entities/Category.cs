using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Publishing.Models.Entities;

public class Category : BaseModel
{
    public string Name { get; set; }
    
    public ICollection<Post> Posts { get; set; }
    
}