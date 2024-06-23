using FitShirt.Domain.Designing.Models.Queries;
using FitShirt.Domain.Purchasing.Models.Commands;
using FitShirt.Domain.Purchasing.Models.Queries;
using FitShirt.Domain.Purchasing.Models.Responses;
using FitShirt.Domain.Purchasing.Services;
using FitShirt.Presentation.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Purchasing.Controllers;

[ApiController]
[Route("api/v1/purchases")]
[Produces("application/json")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseCommandService _purchaseCommandService;
    private readonly IPurchaseQueryService _purchaseQueryService;

    public PurchaseController(IPurchaseCommandService purchaseCommandService,
        IPurchaseQueryService purchaseQueryService)
    {
        _purchaseCommandService = purchaseCommandService;
        _purchaseQueryService = purchaseQueryService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<PurchaseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPurchasesAsync()
    {
        var query = new GetAllPurchasesQuery();
        var result = await _purchaseQueryService.Handle(query);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PurchaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    
    public async Task<IActionResult> GetPurchaseByIdAsync(int id)
    {
        var query = new GetPurchaseByIdQuery(id);
        var result = await _purchaseQueryService.Handle(query);

        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<PurchaseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [Route("search-by-user")]
    public async Task<IActionResult> GetPurchasesByUserIdAsync(int userId)
    {
        var query = new GetPurchaseByUserIdQuery(userId);
        var result = await _purchaseQueryService.Handle(query);

        return Ok(result);
    }
    
    
    [HttpPost]
    [ProducesResponseType(typeof(PurchaseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> PostPurchaseAsync([FromBody] CreatePurchaseCommand command)
    {
        var result = await _purchaseCommandService.Handle(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }


}