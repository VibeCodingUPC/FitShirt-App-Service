using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Messaging.Models.Aggregates;

public class Message : BaseModel
{   
    public string Content { get; set; } = string.Empty;
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;
    public int ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;
    
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}