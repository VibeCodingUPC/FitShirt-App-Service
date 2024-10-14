using FitShirt.Domain.Designing.Models.Queries;
using FitShirt.Domain.Purchasing.Models.Commands;
using FitShirt.Domain.Purchasing.Models.Queries;
using FitShirt.Domain.Purchasing.Models.Responses;
using FitShirt.Domain.Purchasing.Services;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Presentation.Errors;
using FitShirt.Presentation.Filter;
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
    
    /// GET: api/v1/purchases
    /// <summary>
    /// Get a List of All Purchases.
    /// </summary>
    /// <response code="200">Returns all the purchases</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Not authorized</response>
    /// <response code="404">If there are no purchases</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<PurchaseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [CustomAuthorize(UserRoles.ADMIN)]
    public async Task<IActionResult> GetPurchasesAsync()
    {
        var query = new GetAllPurchasesQuery();
        var result = await _purchaseQueryService.Handle(query);
        return Ok(result);
    }
    
    /// GET: api/v1/purchases/{id}
    /// <summary>
    /// Get a List of Purchases by Id.
    /// </summary>
    /// <response code="200">Returns the requested purchase</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Not authorized</response>
    /// <response code="404">If the requested purchase was not found</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PurchaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [CustomAuthorize(UserRoles.ADMIN)]
    public async Task<IActionResult> GetPurchaseByIdAsync(int id)
    {
        var query = new GetPurchaseByIdQuery(id);
        var result = await _purchaseQueryService.Handle(query);

        return Ok(result);
    }
    
    /// GET: api/v1/purchases
    /// <summary>
    /// Get a Purchase by UserId.
    /// </summary>
    /// <response code="200">Returns all the purchases made by the user</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Not authorized</response>
    /// <response code="404">If there are no purchases by the user</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [Route("search-by-user")]
    [ProducesResponseType(typeof(IReadOnlyCollection<PurchaseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [CustomAuthorize(UserRoles.ADMIN)]
    public async Task<IActionResult> GetPurchasesByUserIdAsync(int userId)
    {
        var query = new GetPurchaseByUserIdQuery(userId);
        var result = await _purchaseQueryService.Handle(query);

        return Ok(result);
    }
    
    /// POST: api/v1/purchases
    /// <summary>
    /// Buy a Shirt.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/purchases
    ///     {
    ///        "userId": 1,
    ///        "items": [
    ///        {
    ///          "postId": "2",
    ///          "sizeId": "1",
    ///          "quantity": 1
    ///        }
    /// ]
    ///     }
    ///
    /// </remarks>
    /// <response code="401">Not authenticated</response>
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