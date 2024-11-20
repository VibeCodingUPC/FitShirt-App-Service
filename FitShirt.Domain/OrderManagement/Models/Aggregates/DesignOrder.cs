using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.OrderManagement.Models.ValueObjects;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.OrderManagement.Models.Aggregates;

public class DesignOrder : BaseModel
{
    public DateOnly OrderDate { get; private set; }
    public DateOnly? ShippedDate { get; private set; }
    public OrderStatus Status { get; private set; }
    
    public int UserId { get; private set; }
    public User User { get; private set; }
    
    public int DesignId { get; private set; }
    public Design Design { get; private set; }
    
    public DesignOrder() {}

    public DesignOrder(User user, Design design)
    {
        OrderDate = DateOnly.FromDateTime(DateTime.Now);
        Status = OrderStatus.PENDING;
        User = user;
        Design = design;
    }
}