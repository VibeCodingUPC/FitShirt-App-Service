using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Domain.OrderManagement.Models.Aggregates;
using FitShirt.Domain.OrderManagement.Models.Commands;
using FitShirt.Domain.OrderManagement.Models.Responses;
using FitShirt.Domain.OrderManagement.Repositories;
using FitShirt.Domain.OrderManagement.Services;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Repositories;

namespace FitShirt.Application.OrderManagement.Features.CommandServices;

public class DesignOrderCommandService : IDesignOrderCommandService
{
    private readonly IDesignRepository _designRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDesignOrderRepository _designOrderRepository;
    private readonly IMapper _mapper;

    public DesignOrderCommandService(IDesignRepository designRepository, IUserRepository userRepository, IDesignOrderRepository designOrderRepository, IMapper mapper)
    {
        _designRepository = designRepository;
        _userRepository = userRepository;
        _designOrderRepository = designOrderRepository;
        _mapper = mapper;
    }

    public async Task<DesignOrderResponse> Handle(CreateDesignOrderCommand command)
    {
        var design = await _designRepository.GetByIdAsync(command.DesignId);

        if (design == null)
        {
            throw new NotFoundEntityIdException(nameof(Design), command.DesignId);
        }

        var seller = await _userRepository.GetDetailedUserInformationAsync(command.SellerId);

        if (seller == null)
        {
            throw new NotFoundEntityIdException(nameof(User), command.SellerId);
        }

        var designOrderEntity = new DesignOrder(seller, design);

        await _designOrderRepository.SaveAsync(designOrderEntity);

        var designOrderResponse = _mapper.Map<DesignOrderResponse>(designOrderEntity);

        return designOrderResponse;
    }
}