using System.Drawing;
using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Models.Commands;
using FitShirt.Domain.Designing.Models.Entities;
using FitShirt.Domain.Designing.Models.Responses;
using FitShirt.Domain.Designing.Repositories;
using FitShirt.Domain.Designing.Services;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Application.Designing.Features.CommandServices;

public class DesignCommandService : IDesignCommandService
{
    private readonly IDesignRepository _designRepository;
    private readonly IUserRepository _userRepository;
    private readonly IShieldRepository _shieldRepository;
    private readonly IColorRepository _colorRepository;
    private readonly IMapper _mapper;

    public DesignCommandService(IDesignRepository designRepository, IUserRepository userRepository, IColorRepository colorRepository, IShieldRepository shieldRepository, IMapper mapper)
    {
        _designRepository = designRepository;
        _userRepository = userRepository;
        _colorRepository = colorRepository;
        _shieldRepository = shieldRepository;
        _mapper = mapper;
    }

    public async Task<DesignResponse> Handle(CreateDesignCommand command)
    {
        var designEntity = _mapper.Map<CreateDesignCommand, Design>(command);

        var user = await _userRepository.GetDetailedUserInformationAsync(command.UserId);
        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), command.UserId);
        }
        if (user.Role.Name == UserRoles.SELLER)
        {
            throw new ArgumentException("Sellers are not allowed to create designs");
        }
        designEntity.User = user;

        var shield = await _shieldRepository.GetByIdAsync(command.ShieldId);
        if (shield == null)
        {
            throw new NotFoundEntityIdException(nameof(Shield), command.ShieldId);
        }

        designEntity.Shield = shield;

        var primaryColor = await _colorRepository.GetByIdAsync(command.PrimaryColorId);
        if (primaryColor == null)
        {
            throw new NotFoundEntityIdException(nameof(Color), command.PrimaryColorId);
        }

        designEntity.PrimaryColor = primaryColor;
        
        var secondaryColor = await _colorRepository.GetByIdAsync(command.SecondaryColorId);
        if (secondaryColor == null)
        {
            throw new NotFoundEntityIdException(nameof(Color), command.SecondaryColorId);
        }
        
        designEntity.SecondaryColor = secondaryColor;
        
        var tertiaryColor = await _colorRepository.GetByIdAsync(command.TertiaryColorId);
        if (tertiaryColor == null)
        {
            throw new NotFoundEntityIdException(nameof(Color), command.TertiaryColorId);
        }
        
        designEntity.TertiaryColor = tertiaryColor;

        var designWithSameName = await _designRepository.GetDesignByName(command.Name);
        if (designWithSameName != null)
        {
            throw new DuplicatedEntityAttributeException(nameof(Design), nameof(Design.Name),command.Name);
        }

        if (command.imageUrl == null)
        {
            designEntity.Image =
                "https://static.vecteezy.com/system/resources/previews/021/095/974/non_2x/white-t-shirt-free-png.png";
        }
        else
        {
            designEntity.Image = command.imageUrl;
        }

        await _designRepository.SaveAsync(designEntity);

        var designResponse = _mapper.Map<DesignResponse>(designEntity);

        return designResponse;

    }

    public async Task<DesignResponse> Handle(int id, UpdateDesignCommand command)
    {
        var designToUpdate = await _designRepository.GetByIdAsync(id);
        if (designToUpdate == null)
        {
            throw new NotFoundEntityIdException(nameof(Design), id);
        }
        
        var user = await _userRepository.GetDetailedUserInformationAsync(command.UserId);
        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), command.UserId);
        }
        if (user.Role.Name == UserRoles.SELLER)
        {
            throw new ArgumentException("Sellers are not allowed to create designs");
        }

        var shield = await _shieldRepository.GetByIdAsync(command.ShieldId);
        if (shield == null)
        {
            throw new NotFoundEntityIdException(nameof(Shield), command.ShieldId);
        }

        var primaryColor = await _colorRepository.GetByIdAsync(command.PrimaryColorId);
        if (primaryColor == null)
        {
            throw new NotFoundEntityIdException(nameof(Color), command.PrimaryColorId);
        }
        
        var secondaryColor = await _colorRepository.GetByIdAsync(command.SecondaryColorId);
        if (secondaryColor == null)
        {
            throw new NotFoundEntityIdException(nameof(Color), command.SecondaryColorId);
        }
        
        var tertiaryColor = await _colorRepository.GetByIdAsync(command.TertiaryColorId);
        if (tertiaryColor == null)
        {
            throw new NotFoundEntityIdException(nameof(Color), command.TertiaryColorId);
        }

        if (command.Name != designToUpdate.Name)
        {
            var designWithSameName = await _designRepository.GetDesignByName(command.Name);
            if (designWithSameName != null)
            {
                throw new DuplicatedEntityAttributeException(nameof(Design), nameof(Design.Name), command.Name);
            }
        }

        _mapper.Map(command, designToUpdate, typeof(UpdateDesignCommand), typeof(Design));

        await _designRepository.ModifyAsync(designToUpdate);

        var designResponse = _mapper.Map<DesignResponse>(designToUpdate);

        return designResponse;

    }

    public async Task<bool> Handle(DeleteDesignCommand command)
    {
        var design = await _designRepository.GetByIdAsync(command.Id);
        if (design == null)
        {
            throw new NotFoundEntityIdException(nameof(Design), command.Id);
        }

        return await _designRepository.DeleteAsync(command.Id);
    }
}