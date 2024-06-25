using AutoMapper;
using FitShirt.Application.Publishing.Features.QueryServices;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Publishing.Models.Queries;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Models.Responses;
using Moq;

namespace FitShirt.Application.Test.Publishing.Features.QueryServices;

public class PostQueryServiceTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PostQueryService _postQueryService;

    public PostQueryServiceTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();

        _postQueryService = new PostQueryService(
            _postRepositoryMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_GetAllPosts_ReturnsShirtResponseCollection()
    {
        // Arrange
        var query = new GetAllPostsQuery();
        var postList = new List<Post>
        {
            new Post { Id = 1, Name = "Post 1" },
            new Post { Id = 2, Name = "Post 2" }
        };

        _postRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(postList);

        var shirtResponseList = new List<ShirtResponse>
        {
            new ShirtResponse { Id = 1, Name = "Post 1" },
            new ShirtResponse { Id = 2, Name = "Post 2" }
        };

        _mapperMock.Setup(m => m.Map<List<ShirtResponse>>(postList)).Returns(shirtResponseList);

        // Act
        var result = await _postQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(shirtResponseList, result);
    }
    
    [Fact]
    public async Task Handle_GetAllPosts_ThrowsNoEntitiesFoundException()
    {
        // Arrange
        var query = new GetAllPostsQuery();
        var emptyPostList = new List<Post>();

        _postRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(emptyPostList);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NoEntitiesFoundException>(() => _postQueryService.Handle(query));
        Assert.Equal(nameof(Post), exception.EntityName);
    }
    
    [Fact]
    public async Task Handle_GetPostById_ReturnsPostResponse()
    {
        // Arrange
        var query = new GetPostByIdQuery(1);
        var post = new Post 
        { 
            Id = 1, 
            Name = "Test Post", 
            PostSizes = new List<PostSize>
            {
                new PostSize { SizeId = 1, Size = new Size { Id = 1, Value = "Size 1" } },
                new PostSize { SizeId = 2, Size = new Size { Id = 2, Value = "Size 2" } }
            }
        };

        var postResponse = new PostResponse { Id = 1, Name = "Test Post" };

        _postRepositoryMock.Setup(r => r.GetPostByIdAsync(query.Id)).ReturnsAsync(post);
        _mapperMock.Setup(m => m.Map<PostResponse>(post)).Returns(postResponse);

        // Act
        var result = await _postQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(postResponse, result);
    }
    
    [Fact]
    public async Task Handle_GetPostById_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var query = new GetPostByIdQuery(1);

        _postRepositoryMock.Setup(r => r.GetPostByIdAsync(query.Id)).ReturnsAsync((Post)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postQueryService.Handle(query));
        Assert.Equal(nameof(Post), exception.EntityName);
        Assert.Equal(query.Id, exception.AttributeValue);
    }
    
    [Fact]
    public async Task Handle_GetPostsByUserId_ReturnsShirtResponseCollection()
    {
        // Arrange
        var query = new GetPostsByUserIdQuery(1);
        var user = new User { Id = 1, Name = "Test User" };
        var postList = new List<Post>
        {
            new Post { Id = 1, Name = "Post 1" },
            new Post { Id = 2, Name = "Post 2" }
        };
        var postListResponse = new List<ShirtResponse>
        {
            new ShirtResponse { Id = 1, Name = "Post 1" },
            new ShirtResponse { Id = 2, Name = "Post 2" }
        };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(query.UserId)).ReturnsAsync(user);
        _postRepositoryMock.Setup(r => r.GetPostsByUserIdAsync(query.UserId)).ReturnsAsync(postList);
        _mapperMock.Setup(m => m.Map<List<ShirtResponse>>(postList)).Returns(postListResponse);

        // Act
        var result = await _postQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(postListResponse.Count, result.Count);
        Assert.Equal(postListResponse, result);
    }
    
    [Fact]
    public async Task Handle_GetPostsByUserId_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var query = new GetPostsByUserIdQuery(1);

        _userRepositoryMock.Setup(r => r.GetByIdAsync(query.UserId)).ReturnsAsync((User)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postQueryService.Handle(query));
        Assert.Equal(nameof(User), exception.EntityName);
        Assert.Equal(query.UserId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task Handle_GetPostsByCategoryAndColorId_ReturnsShirtResponseCollection()
    {
        // Arrange
        var query = new GetPostsByCategoryAndColorIdQuery { CategoryId = 1, ColorId = 1 };
        var postList = new List<Post>
        {
            new Post { Id = 1, Name = "Post 1" },
            new Post { Id = 2, Name = "Post 2" }
        };
        var postListResponse = new List<ShirtResponse>
        {
            new ShirtResponse { Id = 1, Name = "Post 1" },
            new ShirtResponse { Id = 2, Name = "Post 2" }
        };

        _postRepositoryMock.Setup(r => r.SearchByFiltersAsync(query.CategoryId, query.ColorId)).ReturnsAsync(postList);
        _mapperMock.Setup(m => m.Map<List<ShirtResponse>>(postList)).Returns(postListResponse);

        // Act
        var result = await _postQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(postListResponse.Count, result.Count);
        Assert.Equal(postListResponse, result);
    }
    
    [Fact]
    public async Task Handle_GetPostsByCategoryAndColorId_ThrowsNoEntitiesFoundException()
    {
        // Arrange
        var query = new GetPostsByCategoryAndColorIdQuery { CategoryId = 1, ColorId = 1 };
        var emptyPostList = new List<Post>();

        _postRepositoryMock.Setup(r => r.SearchByFiltersAsync(query.CategoryId, query.ColorId)).ReturnsAsync(emptyPostList);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NoEntitiesFoundException>(() => _postQueryService.Handle(query));
        Assert.Equal(nameof(Post), exception.EntityName);
    }
}