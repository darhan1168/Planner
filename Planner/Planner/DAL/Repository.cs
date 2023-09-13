using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Planner.Core.Models;

namespace Planner.DAL;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly AppContext _context;
    private readonly DbSet<T> _dbSet;
    
    public Repository(AppContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();                                                                                                       
    }
    
    public async Task<Result<T>> GetByIdAsync(int id)                                                                                                                                                                                                                  
    {
        try
        {
            var entityById = await _dbSet.FindAsync(id);

            return entityById == null ? new Result<T>(false, $"{nameof(T)} not found") : new Result<T>(true);
        }
        catch (Exception ex)
        {
            return new Result<T>(false, $"Failed to add {nameof(T)} to data base. Error: {ex.Message}");
        }
    }                                                                                                               

    public async Task<Result<bool>> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);

            await _context.SaveChangesAsync();

            return new Result<bool>(true);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"Failed to add {nameof(T)} to data base. Error: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(T entity)
    {
        try
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
            
            await _context.SaveChangesAsync();

            return new Result<bool>(true);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"Failed to delete {nameof(T)} from data base. Error: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new Result<bool>(true);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"Failed to update {nameof(T)} to data base. Error: {ex.Message}");
        }
    }

    public async Task<Result<List<T>>> GetAsync(Expression<Func<T, bool>> filter = null, 
        Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null)
    {
        try
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return new Result<List<T>>(true, await orderBy.Compile()(query).ToListAsync());
            }
            
            return new Result<List<T>>(true, await query.ToListAsync());
        }
        catch (Exception ex)
        {
            return new Result<List<T>>(false, $"Failed to update {nameof(T)} to data base. Error: {ex.Message}");
        }
    }
}