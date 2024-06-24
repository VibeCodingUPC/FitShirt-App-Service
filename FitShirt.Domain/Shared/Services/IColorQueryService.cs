using FitShirt.Domain.Shared.Models.Queries;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Shared.Services;

public interface IColorQueryService
{
    Task<IReadOnlyCollection<ColorResponse>> Handle(GetAllColorsQuery query);
    Task<ColorResponse?> Handle(GetColorByIdQuery query);
    
}