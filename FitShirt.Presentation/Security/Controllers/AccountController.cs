using FitShirt.Domain.Security.Models.Commands;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Security.Services;
using FitShirt.Presentation.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Security.Controllers;

[ApiController]
[Route("api/v1/account")]
public class AccountController : ControllerBase
{
    private readonly IUserCommandService _userCommandService;

    public AccountController(IUserCommandService userCommandService)
    {
        _userCommandService = userCommandService;
    }

    /// POST: api/v1/account/register
    /// <summary>
    /// Create an Account.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/account/register
    ///     {
    ///        "name": "Diego",
    ///        "lastname": "Defilippi",
    ///        "username": "Diegos",
    ///        "role": "CLIENT" or "SELLER"
    ///        "password": "123456789",
    ///        "confirmPassword": "123456789",
    ///        "email": "ddefsan@test.com",
    ///        "cellphone": "999999999"
    ///     }
    /// 
    /// </remarks>
    /// <response code="201">If the user was successfully registered</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="404">If any required entity was not found</response>
    /// <response code="409">If there is any conflict</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _userCommandService.Handle(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    /// POST: api/v1/account/login
    /// <summary>
    /// Log in to an Account.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/account/login
    ///     {
    ///        "username": "Diego",
    ///        "password": "123456789"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">If the user was successfully logged</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="404">If any required entity was not found</response>
    /// <response code="409">If there is any conflict</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _userCommandService.Handle(command);
        return Ok(result);
    }
}