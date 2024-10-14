using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Security.Models.Aggregates;

public abstract class User : BaseModel
{
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Cellphone { get; set; }
    public DateOnly BirthDate { get; set; }
    
    public int RoleId { get; set; }
    public Role Role { get; set; }
}