using FitShirt.Domain.Publishing.Models.Commands;
using FitShirt.Domain.Publishing.Models.Responses;

namespace FitShirt.Domain.Publishing.Services;

public interface IPostCommandService
{
    Task<PostResponse> Handle(CreatePostCommand command);
    Task<PostResponse> Handle(int id, UpdatePostCommand command);
    Task<bool> Handle(DeletePostCommand command);
}