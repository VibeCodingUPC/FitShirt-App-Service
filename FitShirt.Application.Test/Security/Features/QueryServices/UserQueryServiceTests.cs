using AutoMapper;
using FitShirt.Application.Security.Features.QueryServices;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Queries;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Security.Repositories;
using Moq;

namespace FitShirt.Application.Test.Security.Features.QueryServices;

public class UserQueryServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UserQueryService _userQueryService;

    public UserQueryServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        
        _userQueryService = new UserQueryService(
            _userRepositoryMock.Object,
            _mapperMock.Object
        );
    }
    
    [Fact]
    public async Task HandleGetAllUsersQuery_NoUsersFound_ThrowsNoEntitiesFoundException()
    {
        // Arrange
        var query = new GetAllUsersQuery();
        _userRepositoryMock.Setup(repo => repo.GetAllDetailedUserInformationAsync()).ReturnsAsync(new List<User>());

        // Act & Assert
        await Assert.ThrowsAsync<NoEntitiesFoundException>(() => _userQueryService.Handle(query));
    }

    [Fact]
    public async Task HandleGetAllUsersQuery_UsersFound_ReturnsUserResponses()
    {
        // Arrange
        var query = new GetAllUsersQuery();
        var users = new List<User>
        {
            new Client { Id = 1, Name = "User1", Email = "user1@example.com" },
            new Client { Id = 2, Name = "User2", Email = "user2@example.com" }
        };
        var userResponses = new List<UserResponse>
        {
            new UserResponse { Id = 1, Name = "User1", Email = "user1@example.com" },
            new UserResponse { Id = 2, Name = "User2", Email = "user2@example.com" }
        };
        _userRepositoryMock.Setup(repo => repo.GetAllDetailedUserInformationAsync()).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<List<UserResponse>>(users)).Returns(userResponses);

        // Act
        var result = await _userQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userResponses.Count, result.Count);
        Assert.Equal(userResponses, result);
    }
    
    [Fact]
    public async Task HandleGetUserByEmailQuery_UserNotFound_ThrowsNoEntitiesFoundException()
    {
        // Arrange
        var query = new GetUserByEmailQuery ("nonexistent@example.com");
        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(query.email)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<NoEntitiesFoundException>(() => _userQueryService.Handle(query));
    }

    [Fact]
    public async Task HandleGetUserByEmailQuery_UserFound_ReturnsUserResponse()
    {
        // Arrange
        var query = new GetUserByEmailQuery ("user@example.com");
        var user = new Client { Id = 1, Name = "User", Email = "user@example.com" };
        var userResponse = new UserResponse { Id = 1, Name = "User", Email = "user@example.com" };
        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(query.email)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(userResponse);

        // Act
        var result = await _userQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userResponse, result);
    }
    
    [Fact]
    public async Task HandleGetUserByPhoneNumberQuery_UserNotFound_ThrowsNoEntitiesFoundException()
    {
        // Arrange
        var query = new GetUserByPhoneNumberQuery("1234567890");
        _userRepositoryMock.Setup(repo => repo.GetUserByPhoneNumberAsync(query.phoneNumber)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<NoEntitiesFoundException>(() => _userQueryService.Handle(query));
    }

    [Fact]
    public async Task HandleGetUserByPhoneNumberQuery_UserFound_ReturnsUserResponse()
    {
        // Arrange
        var query = new GetUserByPhoneNumberQuery ("1234567890");
        var user = new Client { Id = 1, Name = "User", Cellphone = "1234567890" };
        var userResponse = new UserResponse { Id = 1, Name = "User", Cellphone = "1234567890" };
        _userRepositoryMock.Setup(repo => repo.GetUserByPhoneNumberAsync(query.phoneNumber)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(userResponse);

        // Act
        var result = await _userQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userResponse, result);
    }
    
    [Fact]
    public async Task HandleGetUserByUsernameQuery_UserNotFound_ThrowsNoEntitiesFoundException()
    {
        // Arrange
        var query = new GetUserByUsernameQuery("nonexistentuser");
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(query.username)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<NoEntitiesFoundException>(() => _userQueryService.Handle(query));
    }

    [Fact]
    public async Task HandleGetUserByUsernameQuery_UserFound_ReturnsUserResponse()
    {
        // Arrange
        var query = new GetUserByUsernameQuery("existinguser");
        var user = new Client { Id = 1, Name = "User", Username = "existinguser" };
        var userResponse = new UserResponse { Id = 1, Name = "User", Username = "existinguser" };
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(query.username)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(userResponse);

        // Act
        var result = await _userQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userResponse, result);
    }
}