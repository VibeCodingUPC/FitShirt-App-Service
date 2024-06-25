using FitShirt.Domain.Shared.Models.Queries;
using FitShirt.Domain.Shared.Models.Responses;
using FitShirt.Domain.Shared.Services;
using FitShirt.Presentation.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Shared.Controllers;

[ApiController]
[Route("api/v1/colors")]
[Produces("application/json")]
public class ColorController : ControllerBase
{
    private readonly IColorQueryService _colorQueryService;

    public ColorController(IColorQueryService colorQueryService)
    {
        _colorQueryService = colorQueryService;
    }
    
    /// GET: /api/v1/colors
    /// <summary>
    /// Get a colors list.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyCollection<ColorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoriesAsync()
    {
        var query = new GetAllColorsQuery();
        var result = await _colorQueryService.Handle(query);

        return Ok(result);
    }
    
    /// GET: /api/v1/colors/{id}
    /// <summary>
    /// Get a color by its id.
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ColorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoryByIdAsync(int id)
    {
        var query = new GetColorByIdQuery(id);
        var result = await _colorQueryService.Handle(query);

        return Ok(result);
    }
}