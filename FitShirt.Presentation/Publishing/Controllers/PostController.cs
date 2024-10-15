using FitShirt.Domain.Publishing.Models.Commands;
using FitShirt.Domain.Publishing.Models.Queries;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Publishing.Services;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Shared.Models.Responses;
using FitShirt.Presentation.Errors;
using FitShirt.Presentation.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Publishing.Controllers;

[ApiController]
[Route("api/v1/posts")]
[Produces("application/json")]
public class PostController : ControllerBase
{
    private readonly IPostCommandService _postCommandService;
    private readonly IPostQueryService _postQueryService;

    public PostController(IPostCommandService postCommandService, IPostQueryService postQueryService)
    {
        _postCommandService = postCommandService;
        _postQueryService = postQueryService;
    }

    /// GET: /api/v1/posts
    /// <summary>
    /// Get a created posts list.
    /// </summary>
    /// <response code="200">Returns all the posts</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="404">If there are no posts</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShirtResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostsAsync()
    {
        var query = new GetAllPostsQuery();
        var result = await _postQueryService.Handle(query);

        return Ok(result);
    }

    /// GET: /api/v1/posts/{id}
    /// <summary>
    /// Get a post by its id.
    /// </summary>
    /// <response code="200">Returns the required post</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="404">If there is not any posts</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostByIdAsync(int id)
    {
        var query = new GetPostByIdQuery(id);
        var result = await _postQueryService.Handle(query);

        return Ok(result);
    }

    /// GET: /api/v1/posts/search-by-user
    /// <summary>
    /// Get a posts list by the user.
    /// </summary>
    /// <response code="200">Returns the posts made by the user</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If there are no posts</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShirtResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [Route("search-by-user")]
    public async Task<IActionResult> GetPostsByUserIdAsync(int userId)
    {
        var query = new GetPostsByUserIdQuery(userId);
        var result = await _postQueryService.Handle(query);

        return Ok(result);
    }
    
    /// GET: /api/v1/posts/filter-shirts
    /// <summary>
    /// Get a posts list by category and color
    /// </summary>
    /// <response code="200">Returns the posts by category and color</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="404">If there are no posts</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShirtResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [Route("filter-posts")]
    public async Task<IActionResult> GetFilteredPostsAsync(int? categoryId, int? colorId)
    {
        var query = new GetPostsByCategoryAndColorIdQuery(categoryId, colorId);
        var result = await _postQueryService.Handle(query);

        return Ok(result);
    }

    /// POST: /api/v1/posts
    /// <summary>
    /// Create a Post
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/posts
    ///     {
    ///        "name": "Post 5492",
    ///        "image": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/47/PNG_transparency_demonstration_1.png/640px-PNG_transparency_demonstration_1.png",
    ///        "stock": 90,
    ///        "price": 100,
    ///        "categoryId": 2,
    ///        "colorId": 6,
    ///        "userId": 1,
    ///        "sizeIds": [
    ///              1, 4
    ///         ]
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
    [ProducesResponseType(typeof(PostResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.SELLER)]
    public async Task<IActionResult> PostPostAsync([FromBody] CreatePostCommand command)
    {
        var result = await _postCommandService.Handle(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    /// PUT: /api/v1/posts/{id}
    /// <summary>
    /// Modify a Post
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/v1/posts/{id}
    ///     {
    ///        "name": "Post 5492",
    ///        "image": "https://upload.wikimedia.org/wikipedia/commons/thumb/4/47/PNG_transparency_demonstration_1.png/640px-PNG_transparency_demonstration_1.png",
    ///        "stock": 90,
    ///        "price": 100,
    ///        "categoryId": 2,
    ///        "colorId": 6,
    ///        "userId": 1,
    ///        "sizeIds": [
    ///              1, 4
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns all the posts</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If any required entity was not found</response>
    /// <response code="409">If there is any conflict</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.SELLER)]
    public async Task<IActionResult> PutPostAsync(int id, [FromBody] UpdatePostCommand command)
    {
        var result = await _postCommandService.Handle(id, command);
        return Ok(result);
    }
    
    /// DELETE: api/v1/posts/{id}
    /// <summary>
    /// Delete a Post by id
    /// </summary>
    /// <response code="200">Returns all the posts</response>
    /// <response code="400">If the request is wrong</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="404">If there is no post</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CodeErrorResponse), StatusCodes.Status500InternalServerError)]
    [CustomAuthorize(UserRoles.ADMIN, UserRoles.SELLER)]
    public async Task<IActionResult> DeletePostAsync(int id)
    {
        var command = new DeletePostCommand { Id = id };
        var result = await _postCommandService.Handle(command);
        return Ok(result);
    }
}