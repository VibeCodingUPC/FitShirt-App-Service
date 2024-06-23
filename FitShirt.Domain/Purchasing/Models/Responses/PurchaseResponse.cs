using FitShirt.Domain.Security.Models.Responses;

namespace FitShirt.Domain.Purchasing.Models.Responses;

public class PurchaseResponse
{
    public int Id { get; set; }
    public DateTime PurchaseDate { get; set; }
    public UserResponse User { get; set; }
    public List<ItemResponse> Items { get; set; }
}
