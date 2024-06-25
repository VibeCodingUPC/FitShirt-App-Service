using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Security.Models.Aggregates;

public class User : BaseModel
{
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Cellphone { get; set; }
    public DateOnly BirthDate { get; set; }
    public Address? Address { get; set; }
    
    public int? DebitCardId { get; set; }
    public DebitCard? DebitCard { get; set; }
    
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    public ICollection<Design> Designs { get; set; } = new List<Design>();
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}