using System.Linq.Expressions;
using Planner.Core.Models;

namespace Planner.DAL;

public interface IRepository<T> where T : BaseEntity
{
    Task<Result<T>> GetByIdAsync(int id);
    Task<Result<bool>> AddAsync(T entity);
    Task<Result<bool>> DeleteAsync(T entity);
    Task<Result<bool>> UpdateAsync(T entity);
    Task<Result<T>> GetAsync(Expression<Func<T, bool>> filter = null, 
        Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null);
}