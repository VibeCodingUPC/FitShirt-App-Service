using FitShirt.Domain.Messaging.Models.Aggregates;
using FitShirt.Domain.Messaging.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Messaging.Persistence;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    private readonly FitShirtDbContext _context;

    public MessageRepository(FitShirtDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> SendMessageAsync(string content, int senderId, int receiverId)
    {
        var message = new Message
        {
            Content = content,
            SenderId = senderId,
            ReceiverId = receiverId,
            SentAt = DateTime.UtcNow
        };

        await _context.Messages.AddAsync(message);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteMessageAsync(int messageId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message == null || !message.IsEnable) return false;

        message.IsEnable = false;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateMessageAsync(int messageId, string newContent)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message == null || !message.IsEnable) return false;

        message.Content = newContent;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(int userId, int contactId)
    {
        return await _context.Messages
            .Where(m =>
                m.IsEnable &&
                ((m.SenderId == userId && m.ReceiverId == contactId) ||
                 (m.SenderId == contactId && m.ReceiverId == userId)))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<Message?> GetMessageByIdAsync(int messageId)
    {
        return await _context.Messages
            .Where(m => m.IsEnable && m.Id == messageId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Message>> GetAllMessagesAsync()
    {
        return await _context.Messages
            .Where(m => m.IsEnable)
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();
    }
}
