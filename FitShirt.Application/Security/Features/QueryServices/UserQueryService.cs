using AutoMapper;
using FitShirt.Application.Shared.Exceptions;
using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.Queries;
using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Domain.Security.Services;

namespace FitShirt.Application.Security.Features.QueryServices;

public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository _userRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public UserQueryService(IUserRepository userRepository, IServiceRepository serviceRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _serviceRepository = serviceRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<UserResponse?> Handle(GetAllUsersQuery query)
    {
        var data = await _userRepository.GetAllAsync();
        if (data.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(User));
        }

        var result = _mapper.Map<UserResponse>(data);
        return result;
    }

    public Task<UserResponse?> Handle(GetUserByEmailQuery query)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse?> Handle(GetUserByPhoneNumberQuery query)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse?> Handle(GetUserByUsernameQuery query)
    {
        throw new NotImplementedException();
    }
}