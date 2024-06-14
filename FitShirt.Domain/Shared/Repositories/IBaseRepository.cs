using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Shared.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseModel
{
    Task<IReadOnlyCollection<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity> SaveAsync(TEntity entity);
    Task<TEntity> ModifyAsync(TEntity entity);
    Task<bool> DeleteAsync(int id);
}