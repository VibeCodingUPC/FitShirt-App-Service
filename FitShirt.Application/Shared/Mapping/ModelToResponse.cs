using AutoMapper;
using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Models.Entities;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Purchasing.Models.Aggregates;
using FitShirt.Domain.Purchasing.Models.Entities;
using FitShirt.Domain.Purchasing.Models.Responses;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Application.Shared.Mapping;

public class ModelToResponse : Profile
{
    public ModelToResponse()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<Post, ShirtResponse>();
        CreateMap<Category, CategoryResponse>();
        CreateMap<Color, ColorResponse>();
        CreateMap<Category, CategoryResponse>();
        CreateMap<User, UserResponse>();
        CreateMap<PostSize, PostSizeResponse>();
        CreateMap<Size, SizeResponse>();
        CreateMap<Design, DesignResponse>();
        CreateMap<Design, ShirtResponse>();
        CreateMap<Shield, ShieldResponse>();
        CreateMap<Purchase, PurchaseResponse>();
        CreateMap<Item, ItemResponse>();
    }
}