using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Purchasing.Models.Responses;

public class ItemResponse
{
    public ShirtResponse Post { get; set; }
    public SizeResponse Size { get; set; }
    public int Quantity { get; set; }
}   