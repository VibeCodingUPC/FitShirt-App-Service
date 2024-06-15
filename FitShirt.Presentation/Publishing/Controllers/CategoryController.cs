using AutoMapper;
using FitShirt.Domain.Publishing.Models.Queries;
using FitShirt.Domain.Publishing.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Publishing.Controllers;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryCommandService _categoryCommandService;
    private readonly ICategoryQueryService _categoryQueryService;

    public CategoryController(ICategoryCommandService categoryCommandService, ICategoryQueryService categoryQueryService)
    {
        _categoryCommandService = categoryCommandService;
        _categoryQueryService = categoryQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategoriesAsync()
    {
        var query = new GetAllCategoriesQuery();
        var result = await _categoryQueryService.Handle(query);
        
        // TODO: Verify if list is empty

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryByIdAsync(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await _categoryQueryService.Handle(query);
        
        // TODO: Verify category existence

        return Ok(result);
    }
}