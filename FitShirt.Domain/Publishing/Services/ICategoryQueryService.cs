using FitShirt.Domain.Publishing.Models.Queries;
using FitShirt.Domain.Publishing.Models.Responses;

namespace FitShirt.Domain.Publishing.Services;

public interface ICategoryQueryService
{
    Task<IReadOnlyCollection<CategoryResponse>> Handle(GetAllCategoriesQuery query);
    Task<CategoryResponse?> Handle(GetCategoryByIdQuery query);
}