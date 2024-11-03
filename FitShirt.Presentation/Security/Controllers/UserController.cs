using FitShirt.Domain.Security.Models.Commands;
using FitShirt.Domain.Security.Models.Queries;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Security.Services;
using FitShirt.Presentation.Errors;
using FitShirt.Presentation.Filter;
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
    
    /// GET: /api/v1/users/{id}
    /// <summary>
    /// Get a registered user.
    /// </summary>
    /// <response code="200">Returns all the users</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If there are no users registered</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _userQueryService.Handle(query);
        return Ok(result);
    }
    
    /// GET: /api/v1/users
    /// <summary>
    /// Get a created users list.
    /// </summary>
    /// <response code="200">Returns all the users</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If there are no users registered</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [CustomAuthorize(UserRoles.ADMIN)]
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
    /// <response code="401">Not authenticated</response>
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
    /// <response code="401">Not authenticated</response>
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
    
    /// GET: /api/v1/users/sellers
    /// <summary>
    /// Get a registered sellers list.
    /// </summary>
    /// <response code="200">Returns all the sellers</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Not authorized</response>
    /// <response code="404">If there are no users registered</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet("/api/v1/users/sellers")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.CLIENT)]
    public async Task<IActionResult> GetSellersAsync()
    {
        var query = new GetAllSellersQuery();
        var result = await _userQueryService.Handle(query);
        return Ok(result);
    }
    
    /// GET: /api/v1/users/sellers/{id}
    /// <summary>
    /// Get a registered seller by its id
    /// </summary>
    /// <response code="200">Returns the seller</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Not authorized</response>
    /// <response code="404">If the seller was not found</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet("/api/v1/users/sellers/{sellerId}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.CLIENT)]
    public async Task<IActionResult> GetSellerByIdAsync(int sellerId)
    {
        var query = new GetSellerByIdQuery(sellerId);
        var result = await _userQueryService.Handle(query);
        return Ok(result);
    }
}