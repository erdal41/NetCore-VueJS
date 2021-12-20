using VueJS.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace VueJS.Shared.Data.Abstract
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include);
        Task<T> GetAsyncV2(IList<Expression<Func<T, bool>>> predicates, IList<Expression<Func<T, object>>> includeProperties);
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        Task<IList<T>> GetAllIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<IList<T>> MultiUpdateAsync(IList<T> entity);
        Task DeleteAsync(T entity);
        Task<IList<T>> MultiDeleteAsync(IList<T> entity);
        Task<IList<T>> SearchAsync(IList<Expression<Func<T, bool>>> predicates, params Expression<Func<T, object>>[] includeProperties);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
    }
}