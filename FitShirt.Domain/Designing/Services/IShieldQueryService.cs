using FitShirt.Domain.Designing.Models.Queries;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Designing.Services;

public interface IShieldQueryService
{
    Task<IReadOnlyCollection<ShieldResponse>> Handle(GetAllShieldQuery query);
    Task<ShieldResponse?> Handle(GetShieldByIdQuery query);

}