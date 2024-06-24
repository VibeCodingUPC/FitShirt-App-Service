using FitShirt.Domain.Shared.Models.Queries;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Shared.Services;

public interface ISizeQueryService
{
    Task<IReadOnlyCollection<SizeResponse>> Handle(GetAllSizesQuery query);
    Task<SizeResponse?> Handle(GetSizeByIdQuery query);
}