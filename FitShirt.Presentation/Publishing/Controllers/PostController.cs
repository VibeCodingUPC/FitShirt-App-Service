using AutoMapper;
using FitShirt.Domain.Publishing.Models.Commands;
using FitShirt.Domain.Publishing.Models.Queries;
using FitShirt.Domain.Publishing.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitShirt.Presentation.Publishing.Controllers;

[ApiController]
[Route("api/v1/posts")]
public class PostController : ControllerBase
{
    private readonly IPostCommandService _postCommandService;
    private readonly IPostQueryService _postQueryService;

    public PostController(IPostCommandService postCommandService, IPostQueryService postQueryService)
    {
        _postCommandService = postCommandService;
        _postQueryService = postQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPostsAsync()
    {
        var query = new GetAllPostsQuery();
        var result = await _postQueryService.Handle(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostByIdAsync(int id)
    {
        var query = new GetPostByIdQuery(id);
        var result = await _postQueryService.Handle(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("search-by-user")]
    public async Task<IActionResult> GetPostsByUserIdAsync(int userId)
    {
        var query = new GetPostsByUserIdQuery(userId);
        var result = await _postQueryService.Handle(query);

        return Ok(result);
    }
    
    [HttpGet]
    [Route("filter-posts")]
    public async Task<IActionResult> GetFilteredPostsAsync(int? categoryId, int? colorId)
    {
        var query = new GetPostsByCategoryAndColorIdQuery(categoryId, colorId);
        var result = await _postQueryService.Handle(query);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostPostAsync([FromBody] CreatePostCommand command)
    {
        var result = await _postCommandService.Handle(command);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPostAsync(int id, [FromBody] UpdatePostCommand command)
    {
        var result = await _postCommandService.Handle(id, command);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePostAsync(int id)
    {
        var command = new DeletePostCommand { Id = id };
        var result = await _postCommandService.Handle(command);
        return Ok(result);
    }
}