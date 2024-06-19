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
    public UserCommandService(IUserRepository userRepository, IServiceRepository serviceRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _serviceRepository = serviceRepository;
        _roleRepository = roleRepository;
    }

    public Task<UserResponse> Handle(int id, LoginUserCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse> Handle(RegisterUserCommand command)
    {
        throw new NotImplementedException();
    }
}