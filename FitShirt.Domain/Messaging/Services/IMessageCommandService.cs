using FitShirt.Domain.Messaging.Models.Aggregates;

namespace FitShirt.Domain.Messaging.Services;

public interface IMessageCommandService
{
    Task<bool> SendMessageAsync(string content, int senderId, int receiverId);
    
    Task<bool> DeleteMessageAsync(int messageId);
    
    Task<bool> UpdateMessageAsync(int messageId, string newContent);
    
    Task<IEnumerable<Message>> GetMessagesAsync(int userId, int contactId);
}