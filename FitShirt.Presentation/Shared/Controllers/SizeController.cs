using FitShirt.Domain.Shared.Models.Queries;
using FitShirt.Domain.Shared.Models.Responses;
using FitShirt.Domain.Shared.Services;
using FitShirt.Presentation.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Shared.Controllers;

[ApiController]
[Route("api/v1/sizes")]
[Produces("application/json")]
public class SizeController : ControllerBase
{
    private readonly ISizeQueryService _sizeQueryService;

    public SizeController(ISizeQueryService sizeQueryService)
    {
        _sizeQueryService = sizeQueryService;
    }
    
    /// GET: /api/v1/sizes
    /// <summary>
    /// Get a sizes list.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyCollection<SizeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoriesAsync()
    {
        var query = new GetAllSizesQuery();
        var result = await _sizeQueryService.Handle(query);

        return Ok(result);
    }
    
    /// GET: /api/v1/sizes/{id}
    /// <summary>
    /// Get a size by its id.
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SizeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoryByIdAsync(int id)
    {
        var query = new GetSizeByIdQuery(id);
        var result = await _sizeQueryService.Handle(query);

        return Ok(result);
    }
}