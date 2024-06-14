using FitShirt.Domain.Publishing.Models.Entities;
using FitShirt.Domain.Purchasing.Models.Entities;

namespace FitShirt.Domain.Shared.Models.Entities;

public class Size : BaseModel
{
    public string Value { get; set; }

    public ICollection<PostSize> Posts { get; set; }
    public ICollection<Item> Items { get; set; }
}