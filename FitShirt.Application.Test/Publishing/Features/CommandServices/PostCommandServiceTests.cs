using AutoMapper;
using FitShirt.Application.Publishing.Features.CommandServices;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Commands;
using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Models.ImageCloudinary;
using FitShirt.Domain.Shared.Repositories;
using FitShirt.Domain.Shared.Services.ImageCloudinary;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FitShirt.Application.Test.Publishing.Features.CommandServices;

public class PostCommandServiceTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IColorRepository> _colorRepositoryMock;
    private readonly Mock<ISizeRepository> _sizeRepositoryMock;
    private readonly Mock<IPostSizeRepository> _postSizeRepositoryMock;
    private readonly Mock<IPostPhotoRepository> _postPhotoRepository;
    private readonly Mock<IManageImageService> _manageImageService;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PostCommandService _postCommandService;

    public PostCommandServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _colorRepositoryMock = new Mock<IColorRepository>();
        _sizeRepositoryMock = new Mock<ISizeRepository>();
        _postRepositoryMock = new Mock<IPostRepository>();
        _postSizeRepositoryMock = new Mock<IPostSizeRepository>();
        _postPhotoRepository = new Mock<IPostPhotoRepository>();
        _manageImageService = new Mock<IManageImageService>();
        _mapperMock = new Mock<IMapper>();

        _postCommandService = new PostCommandService(
            _postRepositoryMock.Object,
            _mapperMock.Object,
            _userRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _colorRepositoryMock.Object,
            _sizeRepositoryMock.Object,
            _postSizeRepositoryMock.Object,
            _manageImageService.Object,
            _postPhotoRepository.Object
        );
    }

    private IFormFile ConstructFormFile()
    {
        var fileName = "testImage.jpg";
        var contentType = "image/jpeg";
        var fileContent = "fake image content"; // Simulación del contenido de la imagen
        var contentStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));

        var formFile = new FormFile(contentStream, 0, contentStream.Length, "Image", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

        return formFile;
    }

    [Fact]
    public async Task HandleCreatePost_PostCreatedSuccessfully_ReturnsPostResponse()
    {
        // Arrange
        var command = new CreatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Test Post",
            Image = ConstructFormFile(),
            SizeIds = new List<int> { 1, 2 }
        };

        var postEntity = new Post();
        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};
        var category = new Category { Id = 1, Name = "Test Category" };
        var color = new Color { Id = 1, Name = "Test Color" };
        var size1 = new Size { Id = 1, Value = "Size 1" };
        var size2 = new Size { Id = 2, Value = "Size 2" };
        var imageResponse = new ImageResponse
        {
            Url = "https://fakeimageurl.com/image.jpg",
            PublicCode = "fakePublicCode"
        };
        
        _mapperMock.Setup(m => m.Map<CreatePostCommand, Post>(command)).Returns(postEntity);
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync(category);
        _colorRepositoryMock.Setup(r => r.GetByIdAsync(command.ColorId)).ReturnsAsync(color);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(size1);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(size2);
        _postRepositoryMock.Setup(r => r.GetPostByName(command.Name)).ReturnsAsync((Post)null);
        
        _manageImageService.Setup(s => s.UploadImage(It.IsAny<ImageData>())).ReturnsAsync(imageResponse);
        _postPhotoRepository.Setup(r => r.SaveAsync(It.IsAny<PostPhoto>()));
        
        var postSizeList = new List<PostSize>
        {
            new PostSize { SizeId = 1, Size = size1 },
            new PostSize { SizeId = 2, Size = size2 }
        };
        
        postEntity.PostSizes = postSizeList;
        
        _postRepositoryMock.Setup(r => r.SaveAsync(postEntity));

        var postResponse = new PostResponse();
        var sizesResponse = new List<PostSizeResponse>();
        _mapperMock.Setup(m => m.Map<PostResponse>(postEntity)).Returns(postResponse);
        _mapperMock.Setup(m => m.Map<List<PostSizeResponse>>(postEntity.PostSizes)).Returns(sizesResponse);

        // Act
        var result = await _postCommandService.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(postResponse, result);
    }
    
    [Fact]
    public async Task HandleCreatePost_UserNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Test Post",
            SizeIds = new List<int> { 1, 2 }
        };

        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId))
            .ReturnsAsync((Seller)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(command));
        
        // Assert
        Assert.Equal(nameof(User), exception.EntityName);
        Assert.Equal(command.UserId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleCreatePost_CategoryNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Test Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};

        _mapperMock.Setup(m => m.Map<CreatePostCommand, Post>(command)).Returns(new Post());
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync((Category)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(command));
        
        // Assert
        Assert.Equal(nameof(Category), exception.EntityName);
        Assert.Equal(command.CategoryId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleCreatePost_ColorNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Test Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};
        var category = new Category { Id = 1, Name = "Test Category" };

        _mapperMock.Setup(m => m.Map<CreatePostCommand, Post>(command)).Returns(new Post());
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync(category);
        _colorRepositoryMock.Setup(r => r.GetByIdAsync(command.ColorId)).ReturnsAsync((Color)null);

        // Act 
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(command));
        
        // Assert
        Assert.Equal(nameof(Color), exception.EntityName);
        Assert.Equal(command.ColorId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleCreatePost_SizeNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new CreatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Test Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};
        var category = new Category { Id = 1, Name = "Test Category" };
        var color = new Color { Id = 1, Name = "Test Color" };
        var size1 = new Size { Id = 1, Value = "Size 1" };

        _mapperMock.Setup(m => m.Map<CreatePostCommand, Post>(command)).Returns(new Post());
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync(category);
        _colorRepositoryMock.Setup(r => r.GetByIdAsync(command.ColorId)).ReturnsAsync(color);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(size1);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Size)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(command));
        
        // Assert
        Assert.Equal(nameof(Size), exception.EntityName);
        Assert.Equal(2, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleCreatePost_DuplicatedPostName_ThrowsDuplicatedEntityAttributeException()
    {
        // Arrange
        var command = new CreatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Test Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};
        var category = new Category { Id = 1, Name = "Test Category" };
        var color = new Color { Id = 1, Name = "Test Color" };
        var size1 = new Size { Id = 1, Value = "Size 1" };
        var size2 = new Size { Id = 2, Value = "Size 2" };
        var existingPost = new Post { Name = "Test Post" };

        _mapperMock.Setup(m => m.Map<CreatePostCommand, Post>(command)).Returns(new Post());
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync(category);
        _colorRepositoryMock.Setup(r => r.GetByIdAsync(command.ColorId)).ReturnsAsync(color);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(size1);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(size2);
        _postRepositoryMock.Setup(r => r.GetPostByName(command.Name)).ReturnsAsync(existingPost);

        // Act 
        var exception = await Assert.ThrowsAsync<DuplicatedEntityAttributeException>(() => _postCommandService.Handle(command));
        
        // Assert
        Assert.Equal(nameof(Post), exception.EntityName);
        Assert.Equal(nameof(Post.Name), exception.AttributeName);
        Assert.Equal(command.Name, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdatePost_PostUpdatedSuccessfully_ReturnsPostResponse()
    {
        // Arrange
        var id = 1;
        var command = new UpdatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Image = ConstructFormFile(),
            Name = "Updated Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var postToUpdate = new Post { Id = id, Name = "Original Post" };
        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};
        var category = new Category { Id = 1, Name = "Test Category" };
        var color = new Color { Id = 1, Name = "Test Color" };
        var size1 = new Size { Id = 1, Value = "Size 1" };
        var size2 = new Size { Id = 2, Value = "Size 2" };
        var postPhoto = new PostPhoto { Id = 1 };
        var imageResponse = new ImageResponse
        {
            Url = "https://fakeimageurl.com/image.jpg",
            PublicCode = "fakePublicCode"
        };

        _postRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(postToUpdate);
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync(category);
        _colorRepositoryMock.Setup(r => r.GetByIdAsync(command.ColorId)).ReturnsAsync(color);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(size1);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(size2);
        _postRepositoryMock.Setup(r => r.GetPostByName(command.Name)).ReturnsAsync((Post)null);
        
        _mapperMock.Setup(m => m.Map(command, postToUpdate, typeof(UpdatePostCommand), typeof(Post)));

        var postSizeList = new List<PostSize>
        {
            new PostSize { SizeId = 1, Size = size1 },
            new PostSize { SizeId = 2, Size = size2 }
        };

        postToUpdate.PostSizes = postSizeList;

        _postSizeRepositoryMock.Setup(r => r.DeleteByPostIdAsync(id));

        _postPhotoRepository.Setup(r => r.GetPostPhotoByPostId(postToUpdate.Id)).ReturnsAsync(postPhoto);
        _postPhotoRepository.Setup(r => r.DeleteAsync(postPhoto.Id));
        
        _manageImageService.Setup(s => s.UploadImage(It.IsAny<ImageData>())).ReturnsAsync(imageResponse);
        _postPhotoRepository.Setup(r => r.SaveAsync(It.IsAny<PostPhoto>()));
        
        _postRepositoryMock.Setup(r => r.ModifyAsync(postToUpdate));

        var postResponse = new PostResponse();
        var sizesResponse = new List<PostSizeResponse>();
        _mapperMock.Setup(m => m.Map<List<PostSizeResponse>>(postToUpdate.PostSizes)).Returns(sizesResponse);
        _mapperMock.Setup(m => m.Map<PostResponse>(postToUpdate)).Returns(postResponse);

        // Act
        var result = await _postCommandService.Handle(id, command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(postResponse, result);
    }
    
    [Fact]
    public async Task HandleUpdatePost_PostNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Updated Post",
            SizeIds = new List<int> { 1, 2 }
        };

        _postRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Post)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(id, command));
        Assert.Equal(nameof(Post), exception.EntityName);
        Assert.Equal(id, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdatePost_UserNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Updated Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var postToUpdate = new Post { Id = id, Name = "Original Post" };

        _postRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(postToUpdate);
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync((User)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(id, command));
        Assert.Equal(nameof(User), exception.EntityName);
        Assert.Equal(command.UserId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdatePost_CategoryNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Updated Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var postToUpdate = new Post { Id = id, Name = "Original Post" };
        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};

        _postRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(postToUpdate);
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync((Category)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(id, command));
        Assert.Equal(nameof(Category), exception.EntityName);
        Assert.Equal(command.CategoryId, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdatePost_ColorNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Updated Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var postToUpdate = new Post { Id = id, Name = "Original Post" };
        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};
        var category = new Category { Id = 1, Name = "Test Category" };

        _postRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(postToUpdate);
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync(category);
        _colorRepositoryMock.Setup(r => r.GetByIdAsync(command.ColorId)).ReturnsAsync((Color)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(id, command));
        Assert.Equal(nameof(Color), exception.EntityName);
        Assert.Equal(command.ColorId, exception.AttributeValue);
    }

    [Fact]
    public async Task HandleUpdatePost_SizeNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var id = 1;
        var command = new UpdatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Updated Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var postToUpdate = new Post { Id = id, Name = "Original Post" };
        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};
        var category = new Category { Id = 1, Name = "Test Category" };
        var color = new Color { Id = 1, Name = "Test Color" };
        var size1 = new Size { Id = 1, Value = "Size 1" };

        _postRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(postToUpdate);
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync(category);
        _colorRepositoryMock.Setup(r => r.GetByIdAsync(command.ColorId)).ReturnsAsync(color);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(size1);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Size)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(id, command));
        Assert.Equal(nameof(Size), exception.EntityName);
        Assert.Equal(2, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleUpdatePost_DuplicatedPostName_ThrowsDuplicatedEntityAttributeException()
    {
        // Arrange
        var id = 1;
        var command = new UpdatePostCommand
        {
            UserId = 1,
            CategoryId = 1,
            ColorId = 1,
            Name = "Updated Post",
            SizeIds = new List<int> { 1, 2 }
        };

        var postToUpdate = new Post { Id = id, Name = "Original Post" };
        var user = new Seller { Id = 1, Name = "Test User", Role = new Role(UserRoles.SELLER)};
        var category = new Category { Id = 1, Name = "Test Category" };
        var color = new Color { Id = 1, Name = "Test Color" };
        var size1 = new Size { Id = 1, Value = "Size 1" };
        var size2 = new Size { Id = 2, Value = "Size 2" };
        var existingPost = new Post { Name = "Updated Post" };

        _postRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(postToUpdate);
        _userRepositoryMock.Setup(r => r.GetDetailedUserInformationAsync(command.UserId)).ReturnsAsync(user);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId)).ReturnsAsync(category);
        _colorRepositoryMock.Setup(r => r.GetByIdAsync(command.ColorId)).ReturnsAsync(color);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(size1);
        _sizeRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(size2);
        _postRepositoryMock.Setup(r => r.GetPostByName(command.Name)).ReturnsAsync(existingPost);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DuplicatedEntityAttributeException>(() => _postCommandService.Handle(id, command));
        Assert.Equal(nameof(Post), exception.EntityName);
        Assert.Equal(nameof(Post.Name), exception.AttributeName);
        Assert.Equal(command.Name, exception.AttributeValue);
    }
    
    [Fact]
    public async Task HandleDeletePost_PostDeletedSuccessfully_ReturnsTrue()
    {
        // Arrange
        var command = new DeletePostCommand { Id = 1 };
        var post = new Post { Id = 1, Name = "Test Post" };
        var postPhoto = new PostPhoto { Id = 1 };

        _postRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(post);
        _postRepositoryMock.Setup(r => r.DeleteAsync(command.Id)).ReturnsAsync(true);
        
        _postPhotoRepository.Setup(r => r.GetPostPhotoByPostId(post.Id)).ReturnsAsync(postPhoto);
        _postPhotoRepository.Setup(r => r.DeleteAsync(postPhoto.Id));

        // Act
        var result = await _postCommandService.Handle(command);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task HandleDeletePost_PostNotFound_ThrowsNotFoundEntityIdException()
    {
        // Arrange
        var command = new DeletePostCommand { Id = 1 };

        _postRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Post)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundEntityIdException>(() => _postCommandService.Handle(command));
        Assert.Equal(nameof(Post), exception.EntityName);
        Assert.Equal(command.Id, exception.AttributeValue);
    }
}