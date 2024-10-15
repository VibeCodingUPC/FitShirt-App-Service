using AutoMapper;
using FitShirt.Application.Purchasing.Exceptions;
using FitShirt.Application.Purchasing.Features.CommandServices;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Models.Commands;
using FitShirt.Domain.Purchasing.Models.Entities;
using FitShirt.Domain.Purchasing.Models.Responses;
using FitShirt.Domain.Purchasing.Repositories;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Repositories;
using Moq;

namespace FitShirt.Application.Test.Purchasing.Features.CommandServices;

public class PurchaseCommandServiceTests
{
    private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<ISizeRepository> _sizeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PurchaseCommandService _purchaseCommandService;

    public PurchaseCommandServiceTests()
    {
        _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _postRepositoryMock = new Mock<IPostRepository>();
        _sizeRepositoryMock = new Mock<ISizeRepository>();
        _mapperMock = new Mock<IMapper>();

        _purchaseCommandService = new PurchaseCommandService(
            _purchaseRepositoryMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _postRepositoryMock.Object,
            _sizeRepositoryMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ValidPurchase_ReturnsPurchaseResponse()
    {
        // Arrange
        var command = new CreatePurchaseCommand
        {
            UserId = 1,
            Items = new List<CreateItemCommand>
            {
                new CreateItemCommand { PostId = 1, SizeId = 1, Quantity = 1 }
            }
        };

        var user = new Client { Id = 1, Name = "Test User" };
        var post = new Post { Id = 1, Stock = 10 };
        var size = new Size { Id = 1, Value = "L" };
        var purchaseEntity = new Purchase { UserId = 1, Items = new List<Item>(), PurchaseDate = DateTime.Now };
        var purchaseResponse = new PurchaseResponse { Id = 1 };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Items[0].PostId)).ReturnsAsync(post);
        _sizeRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Items[0].SizeId)).ReturnsAsync(size);
        
        _mapperMock.Setup(m => m.Map<CreatePurchaseCommand, Purchase>(command)).Returns(purchaseEntity);
        _purchaseRepositoryMock.Setup(repo => repo.SaveAsync(purchaseEntity));
        _mapperMock.Setup(m => m.Map<PurchaseResponse>(purchaseEntity)).Returns(purchaseResponse);

        // Act
        var result = await _purchaseCommandService.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(purchaseResponse, result);
    }
    
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreatePurchaseCommand
        {
            UserId = 1,
            Items = new List<CreateItemCommand>
            {
                new CreateItemCommand { PostId = 1, SizeId = 1, Quantity = 1 }
            }
        };
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync((User)null);
        
        var purchaseEntity = new Purchase { UserId = 1, Items = new List<Item>(), PurchaseDate = DateTime.Now };
        _mapperMock.Setup(m => m.Map<CreatePurchaseCommand, Purchase>(command)).Returns(purchaseEntity);
        
        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _purchaseCommandService.Handle(command));

        // Assert
        Assert.Equal("User", exception.EntityName);
        Assert.Equal(command.UserId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task Handle_PostNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreatePurchaseCommand
        {
            UserId = 1,
            Items = new List<CreateItemCommand>
            {
                new CreateItemCommand { PostId = 1, SizeId = 1, Quantity = 1 }
            }
        };
        
        var user = new Client { Id = 1, Name = "Test User" };
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()));

        var purchaseEntity = new Purchase { UserId = 1, Items = new List<Item>(), PurchaseDate = DateTime.Now };
        _mapperMock.Setup(m => m.Map<CreatePurchaseCommand, Purchase>(command)).Returns(purchaseEntity);

        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _purchaseCommandService.Handle(command));
    }
    
    [Fact]
    public async Task Handle_SizeNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreatePurchaseCommand
        {
            UserId = 1,
            Items = new List<CreateItemCommand>
            {
                new CreateItemCommand { PostId = 1, SizeId = 1, Quantity = 1 }
            }
        };

        var user = new Client { Id = 1, Name = "Test User" };
        var post = new Post { Id = 1, Stock = 10 };
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Items[0].PostId)).ReturnsAsync(post);
        _sizeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()));

        var purchaseEntity = new Purchase { UserId = 1, Items = new List<Item>(), PurchaseDate = DateTime.Now };
        _mapperMock.Setup(m => m.Map<CreatePurchaseCommand, Purchase>(command)).Returns(purchaseEntity);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _purchaseCommandService.Handle(command));
    }
    
    [Fact]
    public async Task Handle_InsufficientStock_ThrowsInsufficientStockException()
    {
        // Arrange
        var command = new CreatePurchaseCommand
        {
            UserId = 1,
            Items = new List<CreateItemCommand>
            {
                new CreateItemCommand { PostId = 1, SizeId = 1, Quantity = 11 }
            }
        };

        var user = new Client { Id = 1, Name = "Test User" };
        var post = new Post { Id = 1, Stock = 10 };
        var size = new Size { Id = 1, Value = "L" };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Items[0].PostId)).ReturnsAsync(post);
        _sizeRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Items[0].SizeId)).ReturnsAsync(size);

        var purchaseEntity = new Purchase { UserId = 1, Items = new List<Item>(), PurchaseDate = DateTime.Now };
        _mapperMock.Setup(m => m.Map<CreatePurchaseCommand, Purchase>(command)).Returns(purchaseEntity);
        
        // Act & Assert
        await Assert.ThrowsAsync<InsufficientStockException>(() => _purchaseCommandService.Handle(command));
    }
}