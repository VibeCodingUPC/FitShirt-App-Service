namespace FitShirt.Domain.Security.Services;

public interface IGoogleCaptchaValidator
{
    Task<bool> ValidateAsync(string captchaResponse);
}