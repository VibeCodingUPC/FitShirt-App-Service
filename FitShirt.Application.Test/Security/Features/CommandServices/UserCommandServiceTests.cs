using AutoMapper;
using FitShirt.Application.Security.Exceptions;
using FitShirt.Application.Security.Features.CommandServices;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Commands;
using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Security.Services;
using Moq;

namespace FitShirt.Application.Test.Security.Features.CommandServices;

public class UserCommandServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UserCommandService _userCommandService;
    private readonly Mock<IEncryptService> _encryptServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;

    public UserCommandServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _mapperMock = new Mock<IMapper>();
        _encryptServiceMock = new Mock<IEncryptService>();
        _tokenServiceMock = new Mock<ITokenService>();

        _userCommandService = new UserCommandService(
            _userRepositoryMock.Object,
            _serviceRepositoryMock.Object,
            _roleRepositoryMock.Object,
            _mapperMock.Object,
            _encryptServiceMock.Object,
            _tokenServiceMock.Object
        );
    }
    
    [Fact]
    public async Task HandleLoginUserCommand_ValidCredentials_ReturnsUserResponse()
    {
        // Arrange
        var command = new LoginUserCommand
        {
            Username = "testuser",
            Password = "correctpassword"
        };

        var userInDatabase = new User
        {
            Username = command.Username,
            Password = command.Password
        };

        var detailedUser = new User();
        var expectedToken = "generated_token";

        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(command.Username)).ReturnsAsync(userInDatabase);
        _encryptServiceMock.Setup(service => service.Verify(command.Password, userInDatabase.Password))
            .Returns(true);
        _userRepositoryMock.Setup(repo => repo.GetDetailedUserInformationAsync(userInDatabase.Id))
            .ReturnsAsync(detailedUser);
        _tokenServiceMock.Setup(service => service.GenerateToken(detailedUser))
            .Returns(expectedToken);

        // Act
        var result = await _userCommandService.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result);
    }
    
    [Fact]
    public async Task HandleLoginUserCommand_UserNotFound_ThrowsNotFoundEntityAttributeException()
    {
        // Arrange
        var command = new LoginUserCommand
        {
            Username = "testuser",
            Password = "somepassword"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(command.Username)).ReturnsAsync((User)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityAttributeException>(() => _userCommandService.Handle(command));

        // Assert
        Assert.Equal(nameof(User), exception.EntityName);
        Assert.Equal(nameof(command.Username), exception.AttributeName);
        Assert.Equal(command.Username, exception.AttributeValue);
        _userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(command.Username), Times.Once);
    }
    
    [Fact]
    public async Task HandleLoginUserCommand_IncorrectPassword_ThrowsException()
    {
        // Arrange
        var command = new LoginUserCommand
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        var userInDatabase = new User
        {
            Username = command.Username,
            Password = "correctpassword"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(command.Username)).ReturnsAsync(userInDatabase);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _userCommandService.Handle(command));

        // Assert
        Assert.Equal("Incorrect password", exception.Message);
        _userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(command.Username), Times.Once);
    }
    
    [Fact]
    public async Task HandleRegisterUser_UserCreatedSuccessfully_ReturnsUserResponse()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Cellphone = "1234567890",
            Username = "testuser",
            Birthdate = DateOnly.Parse("2000-01-01")
        };

        var userEntity = new User();
        var clientRole = new Role { Id = 1, Name = "Client" };
        var freeService = new Service { Id = 1, Name = "Free Service" };

        _mapperMock.Setup(m => m.Map<RegisterUserCommand, User>(command)).Returns(userEntity);
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(command.Email)).ReturnsAsync((User)null);
        _userRepositoryMock.Setup(r => r.GetUserByPhoneNumberAsync(command.Cellphone)).ReturnsAsync((User)null);
        _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(command.Username)).ReturnsAsync((User)null);
        _roleRepositoryMock.Setup(r => r.GetClientRoleAsync()).ReturnsAsync(clientRole);
        _serviceRepositoryMock.Setup(r => r.GetFreeServiceAsync()).ReturnsAsync(freeService);
        _userRepositoryMock.Setup(r => r.SaveAsync(userEntity));

        var userResponse = new UserResponse();
        _mapperMock.Setup(m => m.Map<UserResponse>(userEntity)).Returns(userResponse);

        // Act
        var result = await _userCommandService.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userResponse, result);
    }

    [Fact]
    public async Task HandleRegisterUser_DuplicatedEmail_ThrowsDuplicatedUserEmailException()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "duplicate@example.com",
            Cellphone = "1234567890",
            Username = "testuser",
            Birthdate = DateOnly.Parse("2000-01-01")
        };

        var existingUser = new User { Email = command.Email };
    
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(command.Email)).ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DuplicatedUserEmailException>(() => _userCommandService.Handle(command));
        Assert.Equal(command.Email, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleRegisterUser_DuplicatedPhoneNumber_ThrowsDuplicatedUserCellphoneException()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Cellphone = "duplicatePhone",
            Username = "testuser",
            Birthdate = DateOnly.Parse("2000-01-01")
        };

        var existingUser = new User { Cellphone = command.Cellphone };
    
        _userRepositoryMock.Setup(r => r.GetUserByPhoneNumberAsync(command.Cellphone)).ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DuplicatedUserCellphoneException>(() => _userCommandService.Handle(command));
        Assert.Equal(command.Cellphone, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleRegisterUser_DuplicatedUsername_ThrowsDuplicatedUserUsernameException()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Cellphone = "1234567890",
            Username = "duplicateUsername",
            Birthdate = DateOnly.Parse("2000-01-01")
        };

        var existingUser = new User { Username = command.Username };
    
        _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(command.Username)).ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DuplicatedUserUsernameException>(() => _userCommandService.Handle(command));
        Assert.Equal(command.Username, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleRegisterUser_AgeLowerThan18_ThrowsUserLowerAgeException()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Cellphone = "1234567890",
            Username = "testuser",
            Birthdate = DateOnly.Parse("2010-01-01")
        };

        // Act & Assert
        await Assert.ThrowsAsync<UserLowerAgeException>(() => _userCommandService.Handle(command));
    }
}