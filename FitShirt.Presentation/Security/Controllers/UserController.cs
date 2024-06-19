using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Security.Services;
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
}