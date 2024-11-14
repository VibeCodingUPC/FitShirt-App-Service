using FitShirt.Domain.OrderManagement.Models.Aggregates;
using FitShirt.Domain.Shared.Repositories;

namespace FitShirt.Domain.OrderManagement.Repositories;

public interface IDesignOrderRepository : IBaseRepository<DesignOrder>
{
    Task<IReadOnlyCollection<DesignOrder>> GetDesignsBySellerIdAsync(int sellerId);
}