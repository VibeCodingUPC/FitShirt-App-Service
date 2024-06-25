using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.Designing.Repositories;

public interface IDesignRepository : IBaseRepository<Design>
{
    Task<Design?> GetDesignByIdAsync(int id);
    Task<IReadOnlyCollection<Design>> GetDesignByUserIdAsync(int userId);
    Task<Design?> GetDesignByName(string name);
}