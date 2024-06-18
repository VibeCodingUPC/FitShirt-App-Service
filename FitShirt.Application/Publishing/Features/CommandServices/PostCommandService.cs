using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Commands;
using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Domain.Publishing.Services;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Application.Publishing.Features.CommandServices;

public class PostCommandService : IPostCommandService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IColorRepository _colorRepository;
    private readonly ISizeRepository _sizeRepository;
    private readonly IPostSizeRepository _postSizeRepository;
    private readonly IMapper _mapper;

    public PostCommandService(IPostRepository postRepository, IMapper mapper, IUserRepository userRepository, ICategoryRepository categoryRepository, IColorRepository colorRepository, ISizeRepository sizeRepository, IPostSizeRepository postSizeRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _colorRepository = colorRepository;
        _sizeRepository = sizeRepository;
        _postSizeRepository = postSizeRepository;
        _mapper = mapper;
    }

    public async Task<PostResponse> Handle(CreatePostCommand command)
    {
        var postEntity = _mapper.Map<CreatePostCommand, Post>(command);
        
        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), command.UserId);
        }
        postEntity.User = user;
        
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId);
        if (category == null)
        {
            throw new NotFoundEntityIdException(nameof(Category), command.CategoryId);
        }
        postEntity.Category = category;
        
        var color = await _colorRepository.GetByIdAsync(command.ColorId);
        if (color == null)
        {
            throw new NotFoundEntityIdException(nameof(Color), command.ColorId);
        }
        postEntity.Color = color;
        
        var sizeIds = command.SizeIds;
        List<PostSize> postSizeList = new List<PostSize>();
        foreach (var id in sizeIds)
        {
            var size = await _sizeRepository.GetByIdAsync(id);

            if (size == null)
            {
                throw new NotFoundEntityIdException(nameof(Size), id);
            }

            var postSize = new PostSize
            {
                SizeId = size.Id,
                Size = size
            };
            
            postSizeList.Add(postSize);
        }

        postEntity.PostSizes = postSizeList;
        
        
        var postWithSameName = await _postRepository.GetPostByName(command.Name);
        if (postWithSameName != null)
        {
            throw new DuplicatedEntityAttributeException(nameof(Post), nameof(Post.Name), command.Name);
        }

        await _postRepository.SaveAsync(postEntity);

        var sizesResponse = _mapper.Map<List<PostSizeResponse>>(postEntity.PostSizes);
        var postResponse = _mapper.Map<PostResponse>(postEntity);
        postResponse.Sizes = sizesResponse;

        return postResponse;
    }

    public async Task<PostResponse> Handle(int id, UpdatePostCommand command)
    {
        var postToUpdate = await _postRepository.GetByIdAsync(id);
        if (postToUpdate == null)
        {
            throw new NotFoundEntityIdException(nameof(Post), id);
        }
        
        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), command.UserId);
        }
        
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId);
        if (category == null)
        {
            throw new NotFoundEntityIdException(nameof(Category), command.CategoryId);
        }
        
        var color = await _colorRepository.GetByIdAsync(command.ColorId);
        if (color == null)
        {
            throw new NotFoundEntityIdException(nameof(Color), command.ColorId);
        }
        
        var sizeIds = command.SizeIds;
        List<PostSize> postSizeList = new List<PostSize>();
        foreach (var sid in sizeIds)
        {
            var size = await _sizeRepository.GetByIdAsync(sid);

            if (size == null)
            {
                throw new NotFoundEntityIdException(nameof(Size), sid);
            }

            var postSize = new PostSize
            {
                SizeId = size.Id,
                Size = size
            };
            
            postSizeList.Add(postSize);
        }

        if (command.Name != postToUpdate.Name)
        {
            var postWithSameName = await _postRepository.GetPostByName(command.Name);
            if (postWithSameName != null)
            {
                throw new DuplicatedEntityAttributeException(nameof(Post), nameof(Post.Name), command.Name);
            }
        }
        
        _mapper.Map(command, postToUpdate, typeof(UpdatePostCommand), typeof(Post));
        
        postToUpdate.PostSizes = postSizeList;

        await _postSizeRepository.DeleteByPostIdAsync(id);
        await _postRepository.ModifyAsync(postToUpdate);

        var sizesResponse = _mapper.Map<List<PostSizeResponse>>(postToUpdate.PostSizes);
        var postResponse = _mapper.Map<PostResponse>(postToUpdate);
        postResponse.Sizes = sizesResponse;
        
        return postResponse;
    }

    public async Task<bool> Handle(DeletePostCommand command)
    {
        var post = await _postRepository.GetByIdAsync(command.Id);
        if (post == null)
        {
            throw new NotFoundEntityIdException(nameof(Post), command.Id);
        }
        
        return await _postRepository.DeleteAsync(command.Id);
    }
}