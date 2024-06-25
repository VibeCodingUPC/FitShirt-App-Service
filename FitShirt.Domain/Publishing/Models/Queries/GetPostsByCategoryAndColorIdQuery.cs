namespace FitShirt.Domain.Publishing.Models.Queries;

public record GetPostsByCategoryAndColorIdQuery(int? CategoryId = null, int? ColorId = null);