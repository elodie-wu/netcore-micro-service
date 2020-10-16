using MicroService.Common.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Basic.Data
{
    public interface IBaseRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        Task<int> InsertAsync(TEntity entity);
        Task<int> InsertAsync(List<TEntity> entitys);
        Task<int> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TPrimaryKey keyValue);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindEntityAsync(TPrimaryKey keyValue);
        Task<TEntity> FindEntityAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> IQueryable();
        IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate); 
        Task<List<TEntity>> FindListAsync(Pagination pagination);
        Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate, Pagination pagination);
    }
}

