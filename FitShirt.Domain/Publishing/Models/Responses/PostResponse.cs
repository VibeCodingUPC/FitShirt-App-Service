using FitShirt.Domain.Security.Models.Responses;
using FitShirt.Domain.Shared.Models.Responses;

namespace FitShirt.Domain.Publishing.Models.Responses;

public class PostResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public int Stock { get; set; }
    public double Price { get; set; }
    public CategoryResponse Category { get; set; }
    public ColorResponse Color { get; set; }
    public UserResponse User { get; set; }
    public List<PostSizeResponse> Sizes { get; set; }
}