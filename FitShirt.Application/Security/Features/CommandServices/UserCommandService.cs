using AutoMapper;
using FitShirt.Application.Security.Exceptions;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Commands;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Security.Services;

namespace FitShirt.Application.Security.Features.CommandServices;

public class UserCommandService : IUserCommandService
{

    private readonly IUserRepository _userRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public UserCommandService(IUserRepository userRepository, IServiceRepository serviceRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _serviceRepository = serviceRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<UserResponse> Handle(LoginUserCommand command)
    {
        throw new NotImplementedException();
    }

    public async Task<UserResponse> Handle(RegisterUserCommand command)
    {
        var userEntity = _mapper.Map<RegisterUserCommand, User>(command);
        
        var userWithSameEmail = await _userRepository.GetUserByEmailAsync(command.Email);
        if (userWithSameEmail != null)
        {
            throw new DuplicatedUserEmailException(command.Email);
        }
        
        var userWithSamePhoneNumber = await _userRepository.GetUserByPhoneNumberAsync(command.Cellphone);
        if (userWithSamePhoneNumber != null)
        {
            throw new DuplicatedUserCellphoneException(command.Cellphone);
        }
        
        var userWithSameUsername = await _userRepository.GetUserByUsernameAsync(command.Username);
        if (userWithSameUsername != null)
        {
            throw new DuplicatedUserUsernameException(command.Username);
        }
        
        if (IsAgeLowerThan18(command.Birthdate))
        {
            throw new UserLowerAgeException();
        }

        var clientRole = await _roleRepository.GetClientRoleAsync();
        var freeService = await _serviceRepository.GetFreeServiceAsync();
        
        userEntity.RoleId = clientRole!.Id;
        userEntity.ServiceId = freeService!.Id;

        await _userRepository.SaveAsync(userEntity);

        var userResponse = _mapper.Map<UserResponse>(userEntity);

        return userResponse;
    }

    public async Task<UserResponse> Handle(int id, UpdateUserCommand command)
    {
        var userToUpdate = await _userRepository.GetByIdAsync(id);
        if (userToUpdate == null)
        {
            throw new NotFoundEntityIdException(nameof(User), id);
        }
        var userWithSameEmail = await _userRepository.GetUserByEmailAsync(command.Email);
        if (userWithSameEmail != null)
        {
            throw new DuplicatedUserEmailException(command.Email);
        }
        
        var userWithSamePhoneNumber = await _userRepository.GetUserByPhoneNumberAsync(command.Cellphone);
        if (userWithSamePhoneNumber != null)
        {
            throw new DuplicatedUserCellphoneException(command.Cellphone);
        }
        
        var userWithSameUsername = await _userRepository.GetUserByUsernameAsync(command.Username);
        if (userWithSameUsername != null)
        {
            throw new DuplicatedUserUsernameException(command.Username);
        }
        
        if (IsAgeLowerThan18(command.Birthdate))
        {
            throw new UserLowerAgeException();
        }

        _mapper.Map(command, userToUpdate, typeof(UpdateUserCommand), typeof(User));

        await _userRepository.ModifyAsync(userToUpdate);
        var userResponse = _mapper.Map<UserResponse>(userToUpdate);
        return userResponse;

    }

    public async Task<bool> Handle(DeleteUserCommand command)
    {
        var user = await _userRepository.GetByIdAsync(command.Id);
        if (user == null)
        {
            throw new NotFoundEntityIdException(nameof(User), command.Id);
        }

        return await _userRepository.DeleteAsync(command.Id);
    }

    private bool IsAgeLowerThan18(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - birthDate.Year;
        
        if (birthDate > today.AddYears(-age))
        {
            age--;
        }

        return age < 18;
    }
}