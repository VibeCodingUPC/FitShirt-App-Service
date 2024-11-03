using AutoMapper;
using FitShirt.Application.Purchasing.Features.QueryServices;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Models.Queries;
using FitShirt.Domain.Purchasing.Models.Responses;
using FitShirt.Domain.Purchasing.Repositories;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Security.Repositories;
using Moq;

namespace FitShirt.Application.Test.Purchasing.Features.QueryServices;

public class PurchaseQueryServiceTests
{
    private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PurchaseQueryService _purchaseQueryService;

    public PurchaseQueryServiceTests()
    {
        _purchaseRepositoryMock = new Mock<IPurchaseRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();

        _purchaseQueryService = new PurchaseQueryService(
            _purchaseRepositoryMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );
    }
    
    [Fact]
    public async Task HandleGetPurchaseByIdQuery_PurchaseNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var query = new GetPurchaseByIdQuery(1);
        _purchaseRepositoryMock.Setup(repo => repo.GetPurchaseByIdAsync(query.Id)).ReturnsAsync((Purchase)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _purchaseQueryService.Handle(query));
    }
    
    [Fact]
    public async Task HandleGetPurchaseByIdQuery_PurchaseFound_ReturnsPurchaseResponse()
    {
        // Arrange
        var query = new GetPurchaseByIdQuery(1);
        var purchase = new Purchase { Id = 1 };
        var purchaseResponse = new PurchaseResponse { Id = 1 };

        _purchaseRepositoryMock.Setup(repo => repo.GetPurchaseByIdAsync(query.Id)).ReturnsAsync(purchase);
        _mapperMock.Setup(m => m.Map<PurchaseResponse>(purchase)).Returns(purchaseResponse);

        // Act
        var result = await _purchaseQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(purchaseResponse, result);
    }
    
    [Fact]
    public async Task HandleGetAllPurchasesQuery_NoPurchasesFound_ThrowsNoEntitiesFoundException()
    {
        // Arrange
        var query = new GetAllPurchasesQuery();
        _purchaseRepositoryMock.Setup(repo => repo.GetAllPurchasesAsync()).ReturnsAsync(new List<Purchase>());

        // Act & Assert
        await Assert.ThrowsAsync<NoEntitiesFoundException>(() => _purchaseQueryService.Handle(query));
    }
    
    [Fact]
    public async Task HandleGetAllPurchasesQuery_PurchasesFound_ReturnsPurchaseResponseCollection()
    {
        // Arrange
        var query = new GetAllPurchasesQuery();
        var purchases = new List<Purchase> { new Purchase { Id = 1 }, new Purchase { Id = 2 } };
        var purchaseResponses = new List<PurchaseResponse> { new PurchaseResponse { Id = 1 }, new PurchaseResponse { Id = 2 } };

        _purchaseRepositoryMock.Setup(repo => repo.GetAllPurchasesAsync()).ReturnsAsync(purchases);
        _mapperMock.Setup(m => m.Map<List<PurchaseResponse>>(purchases)).Returns(purchaseResponses);

        // Act
        var result = await _purchaseQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(purchaseResponses, result);
    }
    
    [Fact]
    public async Task HandleGetPurchaseByUserIdQuery_UserNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var query = new GetPurchaseByUserIdQuery(1);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(query.UserId)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _purchaseQueryService.Handle(query));
    }
    
    [Fact]
    public async Task HandleGetPurchaseByUserIdQuery_PurchasesFound_ReturnsPurchaseResponseCollection()
    {
        // Arrange
        var query = new GetPurchaseByUserIdQuery(1);
        var user = new Client { Id = 1, Name = "Test User", Role = new Role(UserRoles.CLIENT)};
        var purchases = new List<Purchase> { new Purchase { Id = 1 }, new Purchase { Id = 2 } };
        var purchaseResponses = new List<PurchaseResponse> { new PurchaseResponse { Id = 1 }, new PurchaseResponse { Id = 2 } };

        _userRepositoryMock.Setup(repo => repo.GetDetailedUserInformationAsync(query.UserId)).ReturnsAsync(user);
        _purchaseRepositoryMock.Setup(repo => repo.GetPurchasesByUserId(query.UserId)).ReturnsAsync(purchases);
        _mapperMock.Setup(m => m.Map<List<PurchaseResponse>>(purchases)).Returns(purchaseResponses);

        // Act
        var result = await _purchaseQueryService.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(purchaseResponses, result);
    }
}