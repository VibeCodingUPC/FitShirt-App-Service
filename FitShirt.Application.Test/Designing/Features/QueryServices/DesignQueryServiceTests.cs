using AutoMapper;
using FitShirt.Application.Designing.Features.QueryServices;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Models.Queries;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Models.Responses;
using Moq;

namespace FitShirt.Application.Test.Designing.Features.QueryServices;

public class DesignQueryServiceTests
{
    private readonly Mock<IDesignRepository> _designRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DesignQueryService _designQueryService;

    public DesignQueryServiceTests()
    {
        _designRepositoryMock = new Mock<IDesignRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();

        _designQueryService = new DesignQueryService(
            _designRepositoryMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );
    }
    
    [Fact]
    public async Task HandleGetDesignByIdQuery_ValidId_ReturnsDesignResponse()
    {
        // Arrange
        var query = new GetDesignByIdQuery(1);
        var design = new Design { Id = query.Id };

        _designRepositoryMock.Setup(repo => repo.GetDesignByIdAsync(query.Id)).ReturnsAsync(design);
        _mapperMock.Setup(m => m.Map<DesignResponse>(design)).Returns(new DesignResponse { Id = query.Id });

        // Act
        var result = await _designQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(query.Id, result.Id);
    }
    
    [Fact]
    public async Task HandleGetDesignByIdQuery_DesignNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var query = new GetDesignByIdQuery(1);

        _designRepositoryMock.Setup(repo => repo.GetDesignByIdAsync(query.Id)).ReturnsAsync((Design)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designQueryService.Handle(query));

        // Assert
        Assert.Equal(nameof(Design), exception.EntityName);
        Assert.Equal(query.Id, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleGetAllDesignsQuery_DesignsFound_ReturnsShirtResponseCollection()
    {
        // Arrange
        var query = new GetAllDesignsQuery();
        var designs = new List<Design> { new Design { Id = 1 }, new Design { Id = 2 } };

        _designRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(designs);
        _mapperMock.Setup(m => m.Map<List<ShirtResponse>>(designs)).Returns(new List<ShirtResponse>
        {
            new ShirtResponse { Id = 1 },
            new ShirtResponse { Id = 2 }
        });

        // Act
        var result = await _designQueryService.Handle(query);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task HandleGetAllDesignsQuery_NoDesignsFound_ThrowsNoEntitiesFoundException()
    {
        // Arrange
        var query = new GetAllDesignsQuery();
        var designs = new List<Design>();

        _designRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(designs);

        // Act
        var exception = await Assert.ThrowsAsync<NoEntitiesFoundException>(() => _designQueryService.Handle(query));

        // Assert
        Assert.Equal(nameof(Design), exception.EntityName);
    }
    
    [Fact]
    public async Task HandleGetDesignByUserIdQuery_ValidUserId_ReturnsShirtResponseCollection()
    {
        // Arrange
        var query = new GetDesignByUserIdQuery(1);
        var user = new Client { Id = query.UserId };
        var designs = new List<Design> { new Design { Id = 1 }, new Design { Id = 2 } };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(query.UserId)).ReturnsAsync(user);
        _designRepositoryMock.Setup(repo => repo.GetDesignByUserIdAsync(query.UserId)).ReturnsAsync(designs);
        _mapperMock.Setup(m => m.Map<List<ShirtResponse>>(designs)).Returns(new List<ShirtResponse>
        {
            new ShirtResponse { Id = 1 },
            new ShirtResponse { Id = 2 }
        });

        // Act
        var result = await _designQueryService.Handle(query);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task HandleGetDesignByUserIdQuery_UserNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var query = new GetDesignByUserIdQuery(1);

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(query.UserId)).ReturnsAsync((User)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designQueryService.Handle(query));

        // Assert
        Assert.Equal(nameof(User), exception.EntityName);
        Assert.Equal(query.UserId, exception.AttributeValue);
    }
}