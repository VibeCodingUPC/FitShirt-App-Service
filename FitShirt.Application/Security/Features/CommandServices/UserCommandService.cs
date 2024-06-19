using AutoMapper;
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

    public async Task<UserResponse> Handle(int id, LoginUserCommand command)
    {
        

    }

    public async Task<UserResponse> Handle(RegisterUserCommand command)
    {
        var userEntity = _mapper.Map<RegisterUserCommand, User>(command);

        var freeService = await _serviceRepository.GetFreeServiceAsync();

        var userWithSameName = await _userRepository.GetUserByName(command.Username);
        if (userWithSameName != null)
        {
            throw new DuplicatedEntityAttributeException(nameof(User), nameof(User.Username), command.Username);
        }

        await _userRepository.SaveAsync(userEntity);

        var userResponse = _mapper.Map<UserResponse>(userEntity);

        return userResponse;
    }
}