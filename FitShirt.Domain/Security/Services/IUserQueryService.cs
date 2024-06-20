using FitShirt.Domain.Security.Models.Queries;
using FitShirt.Domain.Security.Models.Responses;

namespace FitShirt.Domain.Security.Services;

public interface IUserQueryService
{
    Task<IReadOnlyCollection<UserResponse>> Handle(GetAllUsersQuery query);
    Task<UserResponse?> Handle(GetUserByEmailQuery query);
    Task<UserResponse?> Handle(GetUserByPhoneNumberQuery query);
    Task<UserResponse?> Handle(GetUserByUsernameQuery query);
}