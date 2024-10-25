using FitShirt.Domain.Security.Models.Commands;
using FitShirt.Domain.Security.Models.Responses;

namespace FitShirt.Domain.Security.Services;

public interface IUserCommandService
{
    Task<string> Handle(LoginUserCommand command);
    Task<UserResponse> Handle(RegisterUserCommand command, ValidateCaptchaCommand validateCaptchaCommand);

    Task<UserResponse> Handle(int id, UpdateUserCommand command);
    Task<bool> Handle(DeleteUserCommand command);
}