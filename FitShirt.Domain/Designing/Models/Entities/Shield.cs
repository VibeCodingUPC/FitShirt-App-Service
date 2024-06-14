using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Designing.Models.Entities;

public class Shield : BaseModel
{
    public string NameTeam { get; set; }
    
    public ICollection<Design> Designs { get; set; }
}