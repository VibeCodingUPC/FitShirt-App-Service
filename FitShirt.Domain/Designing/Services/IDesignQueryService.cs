using FitShirt.Domain.Designing.Models.Queries;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Designing.Services;

public interface IDesignQueryService
{
    Task<DesignResponse?> Handle(GetDesignByIdQuery query);
    Task<IReadOnlyCollection<ShirtResponse>> Handle(GetAllDesignsQuery query);
    Task<IReadOnlyCollection<ShirtResponse>> Handle(GetDesignByUserIdQuery query);
    Task<IReadOnlyCollection<ShirtResponse>> Handle(GetDesignsByShieldAndColorIdQuery query);
    Task<ShieldResponse?> Handle(GetShieldByIdQuery query);
}