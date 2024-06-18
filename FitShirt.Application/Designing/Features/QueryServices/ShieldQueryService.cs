using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Designing.Models.Entities;
using FitShirt.Domain.Designing.Models.Queries;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Domain.Designing.Services;

namespace FitShirt.Application.Designing.Features.QueryServices;

public class ShieldQueryService : IShieldQueryService
{
    private readonly IShieldRepository _shieldRepository;
    private readonly IMapper _mapper;

    public ShieldQueryService(IShieldRepository shieldRepository, IMapper mapper)
    {
        _shieldRepository = shieldRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<ShieldResponse>> Handle(GetAllShieldQuery query)
    {
        var data = await _shieldRepository.GetAllAsync();
        if (data.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(Shield));
        }

        var result = _mapper.Map<List<ShieldResponse>>(data);
        return result;
    }

    public async Task<ShieldResponse?> Handle(GetShieldByIdQuery query)
    {
        var data = await _shieldRepository.GetShieldByIdAsync(query.ShieldId);

        if (data == null)
        {
            throw new NotFoundEntityIdException(nameof(Shield), query.ShieldId);
        }

        var shieldResponse = _mapper.Map<ShieldResponse>(data);
        return shieldResponse;
    }
}