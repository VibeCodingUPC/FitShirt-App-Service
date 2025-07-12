using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Messaging.Models.Aggregates;
using FitShirt.Domain.Messaging.Services;
using FitShirt.Domain.Messaging.Repositories;
using FitShirt.Domain.Security.Repositories;

namespace FitShirt.Application.Messaging.Features.CommandServices;

public class MessageCommandService : IMessageCommandService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public MessageCommandService(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> SendMessageAsync(string content, int senderId, int receiverId)
    {
        var sender = await _userRepository.GetByIdAsync(senderId);
        if (sender == null)
            throw new NotFoundEntityIdException("User (Sender)", senderId);

        var receiver = await _userRepository.GetByIdAsync(receiverId);
        if (receiver == null)
            throw new NotFoundEntityIdException("User (Receiver)", receiverId);

        return await _messageRepository.SendMessageAsync(content, senderId, receiverId);
    }

    public async Task<bool> DeleteMessageAsync(int messageId)
    {
        var message = await _messageRepository.GetMessageByIdAsync(messageId);
        if (message == null)
            throw new NotFoundEntityIdException(nameof(Message), messageId);

        return await _messageRepository.DeleteMessageAsync(messageId);
    }

    public async Task<bool> UpdateMessageAsync(int messageId, string newContent)
    {
        var message = await _messageRepository.GetMessageByIdAsync(messageId);
        if (message == null)
            throw new NotFoundEntityIdException(nameof(Message), messageId);

        return await _messageRepository.UpdateMessageAsync(messageId, newContent);
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(int userId, int contactId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundEntityIdException("User", userId);

        var contact = await _userRepository.GetByIdAsync(contactId);
        if (contact == null)
            throw new NotFoundEntityIdException("User", contactId);

        return await _messageRepository.GetMessagesAsync(userId, contactId);
    }
}
