using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Aggregates;

namespace FitShirt.Domain.Shared.Models.Entities;

public class Color : BaseModel
{
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; }
    
    public ICollection<Design> DesignsPrimaryColor { get; set; }
    
    public ICollection<Design> DesignsSecondaryColor { get; set; }
    
    public ICollection<Design> DesignsTertiaryColor { get; set; }
    
}