using FitShirt.Domain.Designing.Models.Queries;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Designing.Services;

public interface IShieldQueryService
{
    Task<IReadOnlyCollection<ShirtResponse>> Handle(GetAllShieldQuery query);
    Task<DesignResponse?> Handle(GetShieldByIdQuery query);

}