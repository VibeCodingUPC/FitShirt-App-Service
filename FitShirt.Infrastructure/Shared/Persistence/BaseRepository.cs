using FitShirt.Domain.Shared.Models.Entities;
using FitShirt.Domain.Shared.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Shared.Persistence;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel
{
    protected readonly FitShirtDbContext _context;

    protected BaseRepository(FitShirtDbContext context)
    {
        _context = context;
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().Where(t => t.IsEnable).ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>().Where(t => t.IsEnable && t.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<TEntity> SaveAsync(TEntity entity)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return entity;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task<TEntity> ModifyAsync(TEntity entity)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return entity;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            entity!.IsEnable = false;
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}