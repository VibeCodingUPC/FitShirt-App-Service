using System.ComponentModel.DataAnnotations;

namespace FitShirt.Domain.Security.Models.Commands;

public class ValidateCaptchaCommand
{
    [Required(ErrorMessage = "Captcha response is required")]
    public string CaptchaResponse { get; set; } = null!;
}