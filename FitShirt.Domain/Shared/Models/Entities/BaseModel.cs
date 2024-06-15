namespace FitShirt.Domain.Shared.Models.Entities;

public abstract class BaseModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public bool IsEnable { get; set; }
}