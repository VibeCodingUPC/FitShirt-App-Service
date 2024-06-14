using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Security.Models.Entities;

public class DebitCard : BaseModel
{
    public string CardNumber { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public string CVV { get; set; }
    
    public User User { get; set; }
}