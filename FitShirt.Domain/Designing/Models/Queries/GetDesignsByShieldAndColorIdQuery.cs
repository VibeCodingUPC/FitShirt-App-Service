namespace FitShirt.Domain.Designing.Models.Queries;

public record GetDesignsByShieldAndColorIdQuery(int? ShieldId = null, int? ColorId = null);