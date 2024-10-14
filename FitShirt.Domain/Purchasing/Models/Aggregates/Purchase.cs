using FitShirt.Domain.Purchasing.Models.Entities;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Purchasing.Models.Aggregates;

public class Purchase : BaseModel
{
    public DateTime PurchaseDate { get; set; }
    public int UserId { get; set; }
    public Client User { get; set; }
    
    public ICollection<Item> Items { get; set; }
}