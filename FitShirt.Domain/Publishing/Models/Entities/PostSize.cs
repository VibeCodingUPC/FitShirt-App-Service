using FitShirt.Domain.Publishing.Models.Aggregates;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Publishing.Models.Entities;

public class PostSize : BaseModel
{
    public int PostId { get; set; }
    public Post Post { get; set; }
    public int SizeId { get; set; }
    public Size Size { get; set; }
}