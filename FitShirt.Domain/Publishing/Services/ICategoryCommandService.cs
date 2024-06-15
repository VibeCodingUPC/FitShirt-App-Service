using FitShirt.Domain.Publishing.Models.Commands;
using FitShirt.Domain.Publishing.Models.Responses;

namespace FitShirt.Domain.Publishing.Services;

public interface ICategoryCommandService
{
    Task<CategoryResponse> Handle(CreateCategoryCommand command);
    Task<CategoryResponse> Handle(int id, UpdateCategoryCommand command);
    Task<bool> Handle(DeleteCategoryCommand command);
}