using AutoMapper;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Commands;

namespace FitShirt.Application.Shared.Mapping;

public class RequestToModel : Profile
{
    public RequestToModel()
    {
        CreateMap<CreatePostCommand, Post>();
        CreateMap<UpdatePostCommand, Post>();
        CreateMap<DeletePostCommand, Post>();
    }
}