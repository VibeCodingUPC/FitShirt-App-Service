using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Publishing.Models.Queries;
using FitShirt.Domain.Publishing.Models.Responses;
using FitShirt.Domain.Publishing.Repositories;
using FitShirt.Domain.Publishing.Services;

namespace FitShirt.Application.Publishing.Features.QueryServices;

public class CategoryQueryService : ICategoryQueryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryQueryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<CategoryResponse>> Handle(GetAllCategoriesQuery query)
    {
        var data = await _categoryRepository.GetAllAsync();
        if (data.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(Category));
        }
        var result = _mapper.Map<List<CategoryResponse>>(data);
        return result;
    }

    public async Task<CategoryResponse?> Handle(GetCategoryByIdQuery query)
    {
        var data = await _categoryRepository.GetByIdAsync(query.Id);
        
        if (data == null)
        {
            throw new NotFoundEntityIdException(nameof(Category), query.Id);
        }
        
        var result = _mapper.Map<CategoryResponse>(data);
        return result;
    }
}