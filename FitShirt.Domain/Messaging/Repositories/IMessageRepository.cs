using FitShirt.Domain.Messaging.Models.Aggregates;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Messaging.Repositories;

public interface IMessageRepository : IBaseRepository<Message>
{
    Task<bool> SendMessageAsync(string content, int senderId, int receiverId);
    
    Task<bool> DeleteMessageAsync(int messageId);
    
    Task<bool> UpdateMessageAsync(int messageId, string newContent);
    
    Task<IEnumerable<Message>> GetMessagesAsync(int userId, int contactId);
    
    Task<Message?> GetMessageByIdAsync(int messageId);
    
    Task<IEnumerable<Message>> GetAllMessagesAsync();
}