namespace FitShirt.Domain.Messaging.Models.Responses;

public class MessageResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public DateTime SentAt { get; set; }
}