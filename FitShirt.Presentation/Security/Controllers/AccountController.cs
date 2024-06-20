using FitShirt.Domain.Security.Models.Commands;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Security.Services;
using FitShirt.Presentation.Errors;
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

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _userCommandService.Handle(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _userCommandService.Handle(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    
}