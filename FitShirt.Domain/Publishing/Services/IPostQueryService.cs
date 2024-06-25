using FitShirt.Domain.Publishing.Models.Queries;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Publishing.Services;

public interface IPostQueryService
{
    Task<IReadOnlyCollection<ShirtResponse>> Handle(GetAllPostsQuery query);
    Task<PostResponse?> Handle(GetPostByIdQuery query);
    Task<IReadOnlyCollection<ShirtResponse>> Handle(GetPostsByUserIdQuery query);
    Task<IReadOnlyCollection<ShirtResponse>> Handle(GetPostsByCategoryAndColorIdQuery query);
}