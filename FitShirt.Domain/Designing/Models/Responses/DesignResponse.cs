using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Designing.Models.Responses;

public class DesignResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public ColorResponse PrimaryColor { get; set; }
    public ColorResponse SecondaryColor { get; set; }
    public ColorResponse TertiaryColor { get; set; }
    public ShieldResponse Shield { get; set; }
    public UserResponse User { get; set; }
}