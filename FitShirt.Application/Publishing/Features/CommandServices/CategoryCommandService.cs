using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Publishing.Models.Commands;
using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Domain.Publishing.Services;

namespace FitShirt.Application.Publishing.Features.CommandServices;

public class CategoryCommandService : ICategoryCommandService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryCommandService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand command)
    {
        var categoryEntity = _mapper.Map<Category>(command);

        await _categoryRepository.SaveAsync(categoryEntity);

        var categoryResponse = _mapper.Map<CategoryResponse>(categoryEntity);
        return categoryResponse;
    }

    public async Task<CategoryResponse> Handle(int id, UpdateCategoryCommand command)
    {
        var categoryToUpdate = await _categoryRepository.GetByIdAsync(id);
        if (categoryToUpdate == null)
        {
            throw new NotFoundEntityIdException(nameof(Category), id);
        }
        
        _mapper.Map(command, categoryToUpdate, typeof(UpdateCategoryCommand), typeof(Category));

        await _categoryRepository.ModifyAsync(categoryToUpdate);
        
        var categoryResponse = _mapper.Map<CategoryResponse>(categoryToUpdate);
        return categoryResponse;
    }

    public async Task<bool> Handle(DeleteCategoryCommand command)
    {
        var categoryToUpdate = await _categoryRepository.GetByIdAsync(command.Id);
        if (categoryToUpdate == null)
        {
            throw new NotFoundEntityIdException(nameof(Category), command.Id);
        }

        return await _categoryRepository.DeleteAsync(command.Id);
    }
}