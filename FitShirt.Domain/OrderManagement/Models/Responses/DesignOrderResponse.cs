using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Security.Models.Responses;

namespace FitShirt.Domain.OrderManagement.Models.Responses;

public class DesignOrderResponse
{
    public int Id { get; set; }
    public DateOnly OrderDate { get; set; }
    public DateOnly? ShippedDate { get; set; }
    public string Status { get; set; }
    public UserResponse User { get; set; }
    public DesignResponse? Design { get; set; }
}