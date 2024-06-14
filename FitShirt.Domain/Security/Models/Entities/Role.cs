using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Security.Models.Entities;

public class Role : BaseModel
{
    public string Name { get; set; }
    public ICollection<User> Users { get; set; }
}