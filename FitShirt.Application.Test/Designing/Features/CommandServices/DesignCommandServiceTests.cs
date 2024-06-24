using AutoMapper;
using FitShirt.Application.Designing.Features.CommandServices;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Models.Commands;
using FitShirt.Domain.Designing.Models.Entities;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Repositories;
using Moq;

namespace FitShirt.Application.Test.Designing.Features.CommandServices;

public class DesignCommandServiceTests
{
    private readonly Mock<IDesignRepository> _designRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IShieldRepository> _shieldRepositoryMock;
    private readonly Mock<IColorRepository> _colorRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DesignCommandService _designCommandService;
    
    public DesignCommandServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _shieldRepositoryMock = new Mock<IShieldRepository>();
        _colorRepositoryMock = new Mock<IColorRepository>();
        _designRepositoryMock = new Mock<IDesignRepository>();
        _mapperMock = new Mock<IMapper>();

        _designCommandService = new DesignCommandService(
            _designRepositoryMock.Object,
            _userRepositoryMock.Object,
            _colorRepositoryMock.Object,
            _shieldRepositoryMock.Object,
            _mapperMock.Object
        );
    }
    
    [Fact]
    public async Task HandleCreateDesign_PostCreatedSuccessfully_ReturnsDesignResponse()
    {
        // Arrange
        var command = new CreateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Test Design"
        };

        var user = new User { Id = 1 };
        var shield = new Shield { Id = 1 };
        var primaryColor = new Color { Id = 1 };
        var secondaryColor = new Color { Id = 2 };
        var tertiaryColor = new Color { Id = 3 };
        var designEntity = new Design { Name = "Test Design" };
        var designResponse = new DesignResponse { Name = "Test Design" };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync(shield);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.PrimaryColorId)).ReturnsAsync(primaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.SecondaryColorId)).ReturnsAsync(secondaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.TertiaryColorId)).ReturnsAsync(tertiaryColor);
        _designRepositoryMock.Setup(repo => repo.GetDesignByName(command.Name)).ReturnsAsync((Design)null);
        _designRepositoryMock.Setup(repo => repo.SaveAsync(It.IsAny<Design>()));

        _mapperMock.Setup(m => m.Map<CreateDesignCommand, Design>(command)).Returns(designEntity);
        _mapperMock.Setup(m => m.Map<DesignResponse>(designEntity)).Returns(designResponse);

        // Act
        var result = await _designCommandService.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(designResponse, result);
    }
    
    [Fact]
    public async Task HandleCreateDesign_UserNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Test Design"
        };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId))
            .ReturnsAsync((User)null);
        
        // Act
        var exception =
            await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(command));
        
        // Assert
        Assert.Equal(nameof(User), exception.EntityName);
        Assert.Equal(command.UserId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleCreateDesign_ShieldNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Test Design"
        };

        var user = new User { Id = 1 };

        _mapperMock.Setup(m => m.Map<CreateDesignCommand, Design>(command)).Returns(new Design());
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId))
            .ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId))
            .ReturnsAsync((Shield)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(command));
    
        // Assert
        Assert.Equal(nameof(Shield), exception.EntityName);
        Assert.Equal(command.ShieldId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleCreateDesign_PrimaryColorNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Test Design"
        };

        var user = new User { Id = 1 };
        var shield = new Shield { Id = 1 };

        _mapperMock.Setup(m => m.Map<CreateDesignCommand, Design>(command)).Returns(new Design());
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync(shield);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.PrimaryColorId)).ReturnsAsync((Color)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(command));
    
        // Assert
        Assert.Equal(nameof(Color), exception.EntityName);
        Assert.Equal(command.PrimaryColorId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleCreateDesign_SecondaryColorNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Test Design"
        };

        var user = new User { Id = 1 };
        var shield = new Shield { Id = 1 };
        var primaryColor = new Color { Id = 1 };

        _mapperMock.Setup(m => m.Map<CreateDesignCommand, Design>(command)).Returns(new Design());
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync(shield);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.PrimaryColorId)).ReturnsAsync(primaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.SecondaryColorId)).ReturnsAsync((Color)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(command));
    
        // Assert
        Assert.Equal(nameof(Color), exception.EntityName);
        Assert.Equal(command.SecondaryColorId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleCreateDesign_TertiaryColorNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Test Design"
        };

        var user = new User { Id = 1 };
        var shield = new Shield { Id = 1 };
        var primaryColor = new Color { Id = 1 };
        var secondaryColor = new Color { Id = 2 };

        _mapperMock.Setup(m => m.Map<CreateDesignCommand, Design>(command)).Returns(new Design());
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync(shield);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.PrimaryColorId)).ReturnsAsync(primaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.SecondaryColorId)).ReturnsAsync(secondaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.TertiaryColorId)).ReturnsAsync((Color)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(command));
    
        // Assert
        Assert.Equal(nameof(Color), exception.EntityName);
        Assert.Equal(command.TertiaryColorId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleCreateDesign_DesignNameDuplicated_ThrowsDuplicatedEntityAttributeException()
    {
        // Arrange
        var command = new CreateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Test Design"
        };

        var user = new User { Id = 1 };
        var shield = new Shield { Id = 1 };
        var primaryColor = new Color { Id = 1 };
        var secondaryColor = new Color { Id = 2 };
        var tertiaryColor = new Color { Id = 3 };
        var existingDesign = new Design { Name = "Test Design" };

        _mapperMock.Setup(m => m.Map<CreateDesignCommand, Design>(command)).Returns(new Design());
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync(shield);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.PrimaryColorId)).ReturnsAsync(primaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.SecondaryColorId)).ReturnsAsync(secondaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.TertiaryColorId)).ReturnsAsync(tertiaryColor);
        _designRepositoryMock.Setup(repo => repo.GetDesignByName(command.Name)).ReturnsAsync(existingDesign);

        // Act
        var exception = await Assert.ThrowsAsync<DuplicatedEntityAttributeException>(() => _designCommandService.Handle(command));
    
        // Assert
        Assert.Equal(nameof(Design), exception.EntityName);
        Assert.Equal(nameof(Design.Name), exception.AttributeName);
        Assert.Equal(command.Name, exception.AttributeValue);
    }
    
    //
    
    [Fact]
    public async Task HandleUpdateDesign_DesignIdNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Updated Design"
        };

        _designRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Design)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(id, command));

        // Assert
        Assert.Equal(nameof(Design), exception.EntityName);
        Assert.Equal(id, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdateDesign_UserNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Updated Design"
        };

        var designToUpdate = new Design { Id = id, Name = "Old Design" };

        _designRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(designToUpdate);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync((User)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(id, command));

        // Assert
        Assert.Equal(nameof(User), exception.EntityName);
        Assert.Equal(command.UserId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdateDesign_ShieldNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Updated Design"
        };

        var designToUpdate = new Design { Id = id, Name = "Old Design" };
        var user = new User { Id = 1 };

        _designRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(designToUpdate);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync((Shield)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(id, command));

        // Assert
        Assert.Equal(nameof(Shield), exception.EntityName);
        Assert.Equal(command.ShieldId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdateDesign_PrimaryColorNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Updated Design"
        };

        var designToUpdate = new Design { Id = id, Name = "Old Design" };
        var user = new User { Id = 1 };
        var shield = new Shield { Id = 1 };

        _designRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(designToUpdate);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync(shield);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.PrimaryColorId)).ReturnsAsync((Color)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(id, command));

        // Assert
        Assert.Equal(nameof(Color), exception.EntityName);
        Assert.Equal(command.PrimaryColorId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdateDesign_SecondaryColorNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Updated Design"
        };

        var designToUpdate = new Design { Id = id, Name = "Old Design" };
        var user = new User { Id = 1 };
        var shield = new Shield { Id = 1 };
        var primaryColor = new Color { Id = 1 };

        _designRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(designToUpdate);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync(shield);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.PrimaryColorId)).ReturnsAsync(primaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.SecondaryColorId)).ReturnsAsync((Color)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(id, command));

        // Assert
        Assert.Equal(nameof(Color), exception.EntityName);
        Assert.Equal(command.SecondaryColorId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdateDesign_TertiaryColorNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Updated Design"
        };

        var designToUpdate = new Design { Id = id, Name = "Old Design" };
        var user = new User { Id = 1 };
        var shield = new Shield { Id = 1 };
        var primaryColor = new Color { Id = 1 };
        var secondaryColor = new Color { Id = 2 };

        _designRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(designToUpdate);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync(shield);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.PrimaryColorId)).ReturnsAsync(primaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.SecondaryColorId)).ReturnsAsync(secondaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.TertiaryColorId)).ReturnsAsync((Color)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(id, command));

        // Assert
        Assert.Equal(nameof(Color), exception.EntityName);
        Assert.Equal(command.TertiaryColorId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdateDesign_DesignNameDuplicated_ThrowsDuplicatedEntityAttributeException()
    {
        // Arrange
        var id = 1;
        var command = new UpdateDesignCommand
        {
            UserId = 1,
            ShieldId = 1,
            PrimaryColorId = 1,
            SecondaryColorId = 2,
            TertiaryColorId = 3,
            Name = "Updated Design"
        };

        var designToUpdate = new Design { Id = id, Name = "Old Design" };
        var user = new User { Id = 1 };
        var shield = new Shield { Id = 1 };
        var primaryColor = new Color { Id = 1 };
        var secondaryColor = new Color { Id = 2 };
        var tertiaryColor = new Color { Id = 3 };
        var existingDesign = new Design { Name = "Updated Design" };

        _designRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(designToUpdate);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _shieldRepositoryMock.Setup(repo => repo.GetByIdAsync(command.ShieldId)).ReturnsAsync(shield);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.PrimaryColorId)).ReturnsAsync(primaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.SecondaryColorId)).ReturnsAsync(secondaryColor);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.TertiaryColorId)).ReturnsAsync(tertiaryColor);
        _designRepositoryMock.Setup(repo => repo.GetDesignByName(command.Name)).ReturnsAsync(existingDesign);

        // Act
        var exception = await Assert.ThrowsAsync<DuplicatedEntityAttributeException>(() => _designCommandService.Handle(id, command));

        // Assert
        Assert.Equal(nameof(Design), exception.EntityName);
        Assert.Equal(nameof(Design.Name), exception.AttributeName);
        Assert.Equal(command.Name, exception.AttributeValue);
    }

    [Fact]
    public async Task HandleDeleteDesign_DesignDeletedSuccessfully_ReturnsTrue()
    {
        // Arrange
        var command = new DeleteDesignCommand { Id = 1 };
        var design = new Design { Id = command.Id };

        _designRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(design);
        _designRepositoryMock.Setup(repo => repo.DeleteAsync(command.Id)).ReturnsAsync(true);

        // Act
        var result = await _designCommandService.Handle(command);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task HandleDeleteDesign_DesignIdNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new DeleteDesignCommand { Id = 1 };

        _designRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync((Design)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _designCommandService.Handle(command));

        // Assert
        Assert.Equal(nameof(Design), exception.EntityName);
        Assert.Equal(command.Id, exception.AttributeValue);
    }
    
}