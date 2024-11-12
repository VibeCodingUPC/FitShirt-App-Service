using FitShirt.Domain.Designing.Models.Commands;
using FitShirt.Domain.Designing.Models.Queries;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Designing.Services;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Shared.Models.Responses;
using FitShirt.Presentation.Errors;
using FitShirt.Presentation.Filter;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Designing.Controllers;


[ApiController]
[Route("api/v1/designs")]
[Produces("application/json")]
public class DesignController : ControllerBase
{
    private readonly IDesignCommandService _designCommandService;
    private readonly IDesignQueryService _designQueryService;

    public DesignController(IDesignCommandService designCommandService, IDesignQueryService designQueryService)
    {
        _designCommandService = designCommandService;
        _designQueryService = designQueryService;
    }

    /// GET: /api/v1/designs
    /// <summary>
    /// Get a created designs list.
    /// </summary>
    /// <response code="200">Returns all the posts</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If there are no posts</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [CustomAuthorize(UserRoles.ADMIN)]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShirtResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDesignsAsync()
    {
        var query = new GetAllDesignsQuery();
        var result = await _designQueryService.Handle(query);
        return Ok(result);
    }

    /// GET: /api/v1/designs/{id}
    /// <summary>
    /// Get a design by its id.
    /// </summary>
    /// <response code="200">Returns the required post</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If there is not any posts</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet("{id}")]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.CLIENT)]
    [ProducesResponseType(typeof(DesignResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDesignByIdAsync(int id)
    {
        var query = new GetDesignByIdQuery(id);
        var result = await _designQueryService.Handle(query);

        return Ok(result);
    }
    
    /// GET: /api/v1/designs/search-by-user
    /// <summary>
    /// Get a designs list by the user.
    /// </summary>
    /// <response code="200">Returns the posts made by the user</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If there are no posts</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.CLIENT)]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShirtResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [Route("search-by-user")]
    public async Task<IActionResult> GetDesignsByUserIdAsync(int userId)
    {
        var query = new GetDesignByUserIdQuery(userId);
        var result = await _designQueryService.Handle(query);

        return Ok(result);
    }
    
    /// POST: /api/v1/designs
    /// <summary>
    /// Create a Design
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/designs
    ///     {
    ///        "name": "RopaPersonalizada",
    ///        "primaryColorId": 1,
    ///        "secondaryColorId": 2,
    ///        "tertiaryColorId": 3,
    ///        "userId": 1,
    ///        "shieldId": 6
    ///     }
    ///
    /// </remarks>
    /// <response code="201">If the post was successfully created</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If any required entity was not found</response>
    /// <response code="409">If there is any conflict</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpPost]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.CLIENT)]
    [ProducesResponseType(typeof(DesignResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostDesignAsync([FromBody] CreateDesignCommand command)
    {
        var result = await _designCommandService.Handle(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    /// PUT: /api/v1/designs/{id}
    /// <summary>
    /// Modify a Design
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/v1/designs/{id}
    ///     {
    ///        "name": "RopaPersonalizada",
    ///        "primaryColorId": 1,
    ///        "secondaryColorId": 2,
    ///        "tertiaryColorId": 3,
    ///        "userId": 1,
    ///        "shieldId": 6
    ///     }
    ///
    /// </remarks>
    /// <response code="201">If the post was successfully created</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If any required entity was not found</response>
    /// <response code="409">If there is any conflict</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpPut("{id}")]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.CLIENT)]
    [ProducesResponseType(typeof(DesignResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutDesignAsync(int id, [FromBody] UpdateDesignCommand command)
    {
        var result = await _designCommandService.Handle(id, command);
        return Ok(result);
    }

    /// DELETE: api/v1/designs/{id}
    /// <summary>
    /// Delete a Design by id
    /// </summary>
    /// <response code="200">Returns all the posts</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If there is no post</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpDelete("{id}")]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.CLIENT)]
    [ProducesResponseType(typeof(DesignResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDesignAsync(int id)
    {
        var command = new DeleteDesignCommand { Id = id };
        var result = await _designCommandService.Handle(command);
        return Ok(result);
    }
    
}