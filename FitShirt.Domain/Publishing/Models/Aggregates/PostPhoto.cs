using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Models.ImageCloudinary;

namespace FitShirt.Domain.Publishing.Models.Aggregates;

public class PostPhoto : BaseModel
{
    public string Url { get; set; }
    public string PublicId { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
    
    public PostPhoto() {}
    
    public PostPhoto(ImageResponse imageResponse, int postId)
    {
        this.Url = imageResponse.Url!;
        this.PublicId = imageResponse.PublicCode!;
        this.PostId = postId;
    }
}