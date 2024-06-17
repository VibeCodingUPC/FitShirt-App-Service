using FitShirt.Domain.Designing.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Designing.Controllers;


[ApiController]
[Route("api/v1/designs")]
[Route("application/json")]
public class DesignController : ControllerBase
{
    private readonly IDesignCommandService _designCommandService;
    private readonly IDesignQueryService _designQueryService;

    public DesignController(IDesignCommandService designCommandService, IDesignQueryService designQueryService)
    {
        _designCommandService = designCommandService;
        _designQueryService = designQueryService;
    }
}