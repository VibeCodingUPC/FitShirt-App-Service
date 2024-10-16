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
    private readonly IMapper _mapper;

    public UserQueryService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<UserResponse>> Handle(GetAllUsersQuery query)
    {
        var data = await _userRepository.GetAllDetailedUserInformationAsync();
        if (data.Count == 0)
        {
            throw new NoEntitiesFoundException(nameof(User));
        }

        var result = _mapper.Map<List<UserResponse>>(data);
        return result;
    }

    public async Task<UserResponse?> Handle(GetUserByEmailQuery query)
    {
        var email = await _userRepository.GetUserByEmailAsync(query.email);
        if (email == null)
        {
            throw new NoEntitiesFoundException(nameof(User));
        }

        var result = _mapper.Map<UserResponse>(email);
        return result;
    }

    public async Task<UserResponse?> Handle(GetUserByPhoneNumberQuery query)
    {
        var phoneNumber = await _userRepository.GetUserByPhoneNumberAsync(query.phoneNumber);
        if (phoneNumber == null)
        {
            throw new NoEntitiesFoundException(nameof(User));
        }

        var result = _mapper.Map<UserResponse>(phoneNumber);
        return result;
    }

    public async Task<UserResponse?> Handle(GetUserByUsernameQuery query)
    {
        var username = await _userRepository.GetUserByUsernameAsync(query.username);
        if (username == null)
        {
            throw new NoEntitiesFoundException(nameof(User));
        }

        var result = _mapper.Map<UserResponse>(username);
        return result;
    }

    public async Task<UserResponse?> Handle(GetUserByIdQuery query)
    {
        var user = await _userRepository.GetDetailedUserInformationAsync(query.Id);
        if (user == null)
        {
            throw new NoEntitiesFoundException(nameof(User));
        }
        
        var result = _mapper.Map<UserResponse>(user);
        return result;
    }
}