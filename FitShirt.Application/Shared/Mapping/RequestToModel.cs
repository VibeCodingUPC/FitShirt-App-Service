using AutoMapper;
using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Models.Commands;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Commands;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Commands;

namespace FitShirt.Application.Shared.Mapping;

public class RequestToModel : Profile
{
    public RequestToModel()
    {
        CreateMap<CreatePostCommand, Post>();
        CreateMap<UpdatePostCommand, Post>();
        CreateMap<DeletePostCommand, Post>();
        CreateMap<CreateDesignCommand, Design>();
        CreateMap<UpdateDesignCommand, Design>();
        CreateMap<DeleteDesignCommand, Design>();
        CreateMap<LoginUserCommand, User>();
        CreateMap<RegisterUserCommand, User>();
        CreateMap<UpdateUserCommand, User>();
        CreateMap<DeleteDesignCommand, User>();
    }
}