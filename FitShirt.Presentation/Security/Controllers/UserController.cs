using FitShirt.Domain.Security.Models.Commands;
using FitShirt.Domain.Security.Models.Queries;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Security.Services;
using FitShirt.Presentation.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Security.Controllers;


[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IUserCommandService _userCommandService;
    private readonly IUserQueryService _userQueryService;

    public UserController(IUserCommandService userCommandService, IUserQueryService userQueryService)
    {
        _userCommandService = userCommandService;
        _userQueryService = userQueryService;
    }
    
    /// GET: /api/v1/users
    /// <summary>
    /// Get a created users list.
    /// </summary>
    /// <response code="200">Returns all the users</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="404">If there are no users registered</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsersAsync()
    {
        var query = new GetAllUsersQuery();
        var result = await _userQueryService.Handle(query);
        return Ok(result);
    }
    
    /// PUT: api/v1/users
    /// <summary>
    /// Modify a User.
    /// </summary>
    /// <response code="200">Returns the required user</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="404">If there is not any user</response>
    /// <response code="409">If there is any conflict</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutUserAsync(int id, [FromBody] UpdateUserCommand command)
    {
        var result = await _userCommandService.Handle(id, command);
        return Ok(result);
    }
    
    
    /// DELETE: api/v1/users
    /// <summary>
    /// Delete a User.
    /// </summary>
    /// <response code="200">Returns the required user</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="404">If there is not any user</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var command = new DeleteUserCommand() { Id = id };
        var result = await _userCommandService.Handle(command);
        return Ok(result);
    }
}