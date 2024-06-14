using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Purchasing.Models.Entities;

public class Item : BaseModel
{
    public int Quantity { get; set; }
    
    public int SizeId { get; set; }
    public Size Size { get; set; }
    
    public int PostId { get; set; }
    public Post Post { get; set; }
    public int PurchaseId { get; set; }
    public Purchase Purchase { get; set; }
}