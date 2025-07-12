namespace FitShirt.Domain.Messaging.Models.Commands;

public class SendMassageCommand
{
    public string Content { get; set; } = string.Empty;

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }
}