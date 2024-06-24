using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Models.Queries;
using FitShirt.Domain.Shared.Models.Responses;
using FitShirt.Domain.Shared.Repositories;
using FitShirt.Domain.Shared.Services;

namespace FitShirt.Application.Shared.Features.QueryServices;

public class ColorQueryService : IColorQueryService
{
    private readonly IColorRepository _colorRepository;
    private readonly IMapper _mapper;

    public ColorQueryService(IColorRepository colorRepository, IMapper mapper)
    {
        _colorRepository = colorRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<ColorResponse>> Handle(GetAllColorsQuery query)
    {
        var data = await _colorRepository.GetAllAsync();
        if (data.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(Color));
        }
        var result = _mapper.Map<List<ColorResponse>>(data);
        return result;
    }

    public async Task<ColorResponse?> Handle(GetColorByIdQuery query)
    {
        var data = await _colorRepository.GetByIdAsync(query.Id);
        
        if (data == null)
        {
            throw new NotFoundEntityIdException(nameof(Color), query.Id);
        }
        
        var result = _mapper.Map<ColorResponse>(data);
        return result;
    }
}