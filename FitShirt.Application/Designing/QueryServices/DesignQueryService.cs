using System.Net.Quic;
using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Models.Queries;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Domain.Designing.Services;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Application.Designing.QueryServices;

public class DesignQueryService : IDesignQueryService
{

    private readonly IDesignRepository _designRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public DesignQueryService(IDesignRepository designRepository, IUserRepository userRepository, IMapper mapper)
    {
        _designRepository = designRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<DesignResponse?> Handle(GetDesignByIdQuery query)
    {
        var data = await _designRepository.GetDesignByIdAsync(query.Id);

        if (data == null)
        {
            throw new NotFoundEntityIdException(nameof(Design), query.Id);
        }

        var designResponse = _mapper.Map<DesignResponse>(data);
        return designResponse;
    }

    public async Task<IReadOnlyCollection<ShirtResponse>> Handle(GetAllDesignsQuery query)
    {
        var data = await _designRepository.GetAllAsync();
        if (data.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(Design));
        }

        var result = _mapper.Map<List<ShirtResponse>>(data);
        return result;
    }

    public async Task<IReadOnlyCollection<ShirtResponse>> Handle(GetDesignByUserIdQuery query)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);

        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), query.UserId);
        }

        var designList = await _designRepository.GetDesignByUserIdAsync(query.UserId);
        var designListResponse = _mapper.Map<List<ShirtResponse>>(designList);
        return designListResponse;
    }

    public async Task<IReadOnlyCollection<ShirtResponse>> Handle(GetDesignsByShieldAndColorIdQuery query)
    {
        var designList = await _designRepository.GetColorByIdAsync(query.ColorId);
    }

    public async Task<ShieldResponse?> Handle(GetShieldByIdQuery query)
    {
        var data = await _designRepository.GetShieldByIdAsync(query.ShieldId);
        if (data == null)
        {
            throw new NotFoundEntityIdException(nameof(Design), query.ShieldId);
        }

        var shieldResponse = _mapper.Map<ShieldResponse>(data);
        return shieldResponse;
    }
}