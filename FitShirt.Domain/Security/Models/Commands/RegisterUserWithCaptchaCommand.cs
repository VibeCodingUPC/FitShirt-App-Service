namespace FitShirt.Domain.Security.Models.Commands;

public class RegisterWithCaptchaCommand
{
    public RegisterUserCommand RegisterUser { get; set; }
    public ValidateCaptchaCommand ValidateCaptcha { get; set; }
}
