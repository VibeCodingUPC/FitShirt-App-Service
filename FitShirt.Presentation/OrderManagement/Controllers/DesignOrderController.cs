using FitShirt.Domain.OrderManagement.Models.Commands;
using FitShirt.Domain.OrderManagement.Models.Queries;
using FitShirt.Domain.OrderManagement.Services;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Presentation.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.OrderManagement.Controllers;

[ApiController]
[Route("api/v1")]
[Produces("application/json")]
public class DesignOrderController : ControllerBase
{
    private readonly IDesignOrderQueryService _designOrderQueryService;
    private readonly IDesignOrderCommandService _designOrderCommandService;

    public DesignOrderController(IDesignOrderQueryService designOrderQueryService, IDesignOrderCommandService designOrderCommandService)
    {
        _designOrderQueryService = designOrderQueryService;
        _designOrderCommandService = designOrderCommandService;
    }
    
    [HttpGet("/design_orders")]
    [CustomAuthorize(UserRoles.ADMIN)]
    public async Task<IActionResult> GetDesignOrdersAsync()
    {
        var query = new GetAllDesignOrdersQuery();
        var result = await _designOrderQueryService.Handle(query);
        return Ok(result);
    }
    
    [HttpGet("/sellers/{sellerId}/design_orders")]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.SELLER)]
    public async Task<IActionResult> GetDesignOrdersBySellerIdAsync(int sellerId)
    {
        var query = new GetDesignOrdersBySellerId(sellerId);
        var result = await _designOrderQueryService.Handle(query);
        return Ok(result);
    }
    
    [HttpGet("/sellers/{sellerId}/design_orders/{designOrderId}")]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.SELLER)]
    public async Task<IActionResult> GetDesignOrderByIdAsync(int sellerId, int designOrderId)
    {
        var query = new GetDesignOrderById(sellerId, designOrderId);
        var result = await _designOrderQueryService.Handle(query);
        return Ok(result);
    }

    [HttpPost("/sellers/{sellerId}/designs/{designId}/design_orders")]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.CLIENT, UserRoles.SELLER)]
    public async Task<IActionResult> PostDesignOrderAsync(int sellerId, int designId)
    {
        var command = new CreateDesignOrderCommand(sellerId, designId);

        var result = await _designOrderCommandService.Handle(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}