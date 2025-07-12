using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Messaging.Services;
using FitShirt.Domain.Messaging.Models.Aggregates;
using FitShirt.Domain.Messaging.Models.Responses;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Presentation.Errors;
using FitShirt.Presentation.Filter;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Messaging.Controllers;

[ApiController]
[Route("api/v1/messages")]
[Produces("application/json")]
public class MessageController : ControllerBase
{
    private readonly IMessageCommandService _messageCommandService;
    private readonly IMapper _mapper;
    
    public MessageController(IMessageCommandService messageCommandService, IMapper mapper)
    {
        _messageCommandService = messageCommandService;
        _mapper = mapper;
    }

    /// <summary>
    /// Send a message between users.
    /// </summary>
    [HttpPost]
    [CustomAuthorize(UserRoles.CLIENT, UserRoles.SELLER, UserRoles.ADMIN)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendMessage([FromQuery] string content, [FromQuery] int senderId, [FromQuery] int receiverId)
    {
        var result = await _messageCommandService.SendMessageAsync(content, senderId, receiverId);
        return Ok(result);
    }

    /// <summary>
    /// Get conversation between two users.
    /// </summary>
    [HttpGet("conversation")]
    [CustomAuthorize(UserRoles.CLIENT, UserRoles.SELLER, UserRoles.ADMIN)]
    [ProducesResponseType(typeof(IEnumerable<MessageResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConversation([FromQuery] int userId, [FromQuery] int contactId)
    {
        var messages = await _messageCommandService.GetMessagesAsync(userId, contactId);
        var responses = _mapper.Map<IEnumerable<MessageResponse>>(messages);
        return Ok(responses);
    }

    /// <summary>
    /// Update an existing message.
    /// </summary>
    [HttpPut("{messageId}")]
    [CustomAuthorize(UserRoles.CLIENT, UserRoles.SELLER, UserRoles.ADMIN)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateMessage([FromRoute] int messageId, [FromQuery] string newContent)
    {
        var result = await _messageCommandService.UpdateMessageAsync(messageId, newContent);
        return Ok(result);
    }

    /// <summary>
    /// Delete a message by id.
    /// </summary>
    [HttpDelete("{messageId}")]
    [CustomAuthorize(UserRoles.CLIENT, UserRoles.SELLER, UserRoles.ADMIN)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteMessage([FromRoute] int messageId)
    {
        var result = await _messageCommandService.DeleteMessageAsync(messageId);
        return Ok(result);
    }
}
