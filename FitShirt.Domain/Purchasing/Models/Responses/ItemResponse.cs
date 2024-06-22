using FitShirt.Domain.Purchasing.Models.Entities;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Purchasing.Models.Responses;

public class ItemResponse
{
    public ShirtVm Post { get; set; }
    public SizeResponse Size { get; set; }
    public int Quantity { get; set; }
}   