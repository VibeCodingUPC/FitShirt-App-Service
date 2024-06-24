using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Models.Queries;
using FitShirt.Domain.Shared.Models.Responses;
using FitShirt.Domain.Shared.Repositories;
using FitShirt.Domain.Shared.Services;

namespace FitShirt.Application.Shared.Features.QueryServices;

public class SizeQueryService : ISizeQueryService
{
    private readonly ISizeRepository _sizeRepository;
    private readonly IMapper _mapper;

    public SizeQueryService(ISizeRepository sizeRepository, IMapper mapper)
    {
        _sizeRepository = sizeRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<SizeResponse>> Handle(GetAllSizesQuery query)
    {
        var data = await _sizeRepository.GetAllAsync();
        if (data.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(Size));
        }
        var result = _mapper.Map<List<SizeResponse>>(data);
        return result;
    }

    public async Task<SizeResponse?> Handle(GetSizeByIdQuery query)
    {
        var data = await _sizeRepository.GetByIdAsync(query.Id);
        
        if (data == null)
        {
            throw new NotFoundEntityIdException(nameof(Size), query.Id);
        }
        
        var result = _mapper.Map<SizeResponse>(data);
        return result;
    }
}