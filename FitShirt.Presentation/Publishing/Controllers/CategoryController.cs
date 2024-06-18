using AutoMapper;
using FitShirt.Domain.Publishing.Models.Queries;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Publishing.Services;
using FitShirt.Presentation.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Publishing.Controllers;

[ApiController]
[Route("api/v1/categories")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryCommandService _categoryCommandService;
    private readonly ICategoryQueryService _categoryQueryService;

    public CategoryController(ICategoryCommandService categoryCommandService, ICategoryQueryService categoryQueryService)
    {
        _categoryCommandService = categoryCommandService;
        _categoryQueryService = categoryQueryService;
    }

    /// GET: /api/v1/categories
    /// <summary>
    /// Get a categories list.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoriesAsync()
    {
        var query = new GetAllCategoriesQuery();
        var result = await _categoryQueryService.Handle(query);

        return Ok(result);
    }
    
    /// GET: /api/v1/categories/{id}
    /// <summary>
    /// Get a category by its id.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoryByIdAsync(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await _categoryQueryService.Handle(query);

        return Ok(result);
    }
}