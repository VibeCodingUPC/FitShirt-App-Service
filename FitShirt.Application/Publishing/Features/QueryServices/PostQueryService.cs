using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Queries;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Domain.Publishing.Services;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Models.Responses;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Application.Publishing.Features.QueryServices;

public class PostQueryService : IPostQueryService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public PostQueryService(IPostRepository postRepository, IUserRepository userRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<ShirtResponse>> Handle(GetAllPostsQuery query)
    {
        var data = await _postRepository.GetShirtsAsync();

        if (data.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(Post));
        }
        
        var result = _mapper.Map<List<ShirtResponse>>(data);
        return result;
    }

    public async Task<PostResponse?> Handle(GetPostByIdQuery query)
    {
        var data = await _postRepository.GetPostByIdAsync(query.Id);
        
        if (data == null)
        {
            throw new NotFoundEntityIdException(nameof(Post), query.Id);
        }
        
        var sizesResponse = _mapper.Map<List<PostSizeResponse>>(data.PostSizes);
        var postResponse = _mapper.Map<PostResponse>(data);
        postResponse.Sizes = sizesResponse;
        
        return postResponse;
    }

    public async Task<IReadOnlyCollection<ShirtResponse>> Handle(GetPostsByUserIdQuery query)
    {
        var user =  await _userRepository.GetByIdAsync(query.UserId);

        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), query.UserId);
        }
        
        var postList = await _postRepository.GetPostsByUserIdAsync(query.UserId);
        var postListResponse = _mapper.Map<List<ShirtResponse>>(postList);
        return postListResponse;
    }

    public async Task<IReadOnlyCollection<ShirtResponse>> Handle(GetPostsByCategoryAndColorIdQuery query)
    {
        var postList = await _postRepository.SearchByFiltersAsync(query.CategoryId, query.ColorId);

        if (postList.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(Post));
        }
        
        var postListResponse = _mapper.Map<List<ShirtResponse>>(postList);
        return postListResponse;
    }
}