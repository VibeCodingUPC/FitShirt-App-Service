using FitShirt.Domain.Designing.Models.Entities;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Designing.Models.Aggregates;

public class Design : BaseModel
{
    public string Name { get; set; }
    public string? Image { get; set; }
    
    public int ShieldId { get; set; }
    public Shield Shield { get; set; }
    
    public int PrimaryColorId { get; set; }
    public Color PrimaryColor { get; set; }
    public int SecondaryColorId { get; set; }
    public Color SecondaryColor { get; set; }
    public int TertiaryColorId { get; set; }
    public Color TertiaryColor { get; set; }
    
    public int UserId { get; set; }
    public Client User { get; set; }
    
}