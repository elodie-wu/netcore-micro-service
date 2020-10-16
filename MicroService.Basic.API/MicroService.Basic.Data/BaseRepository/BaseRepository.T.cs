using MicroService.Common.Page;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MicroService.Basic.Data
{
    public class BaseRepository<TEntity, TPrimaryKey> : IBaseRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        private readonly DbContext _dbContext;
        public BaseRepository(DbContext context)
        {
            _dbContext = context;
        }
        public async Task<bool> DeleteAsync(TPrimaryKey key)
        {
            var entity = await _dbContext.FindAsync<TEntity>(key);
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {

            var entities = _dbContext.Set<TEntity>().Where(predicate);
            _dbContext.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> FindEntityAsync(TPrimaryKey key)
        {
            return await _dbContext.FindAsync<TEntity>(key);
        }

        public async Task<TEntity> FindEntityAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<int> InsertAsync(TEntity entity)
        {
            _dbContext.Add(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public Task<int> InsertAsync(List<TEntity> entities)
        {
            _dbContext.AddRange(entities);
            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<TEntity> IQueryable()
        {
            return _dbContext.Set<TEntity>();

        }

        public IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            return await _dbContext.SaveChangesAsync(); 
        }

        public async Task<List<TEntity>> FindListAsync(Pagination pagination)
        {
            bool isAsc = pagination.isAsc;
            string[] _order = pagination.Sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = _dbContext.Set<TEntity>().AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.TotalCount = tempData.Count();
            tempData = tempData.Skip(pagination.PageSize * (pagination.PageIndex - 1)).Take(pagination.PageSize).AsQueryable();
            return await tempData.ToListAsync();
        }

        public async Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate, Pagination pagination)
        {
            bool isAsc = pagination.isAsc;
            string[] _order = pagination.Sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = _dbContext.Set<TEntity>().Where(predicate);
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.TotalCount = tempData.Count();
            tempData = tempData.Skip(pagination.PageSize * (pagination.PageIndex - 1)).Take(pagination.PageSize).AsQueryable();
            return await tempData.ToListAsync();
        }
    }
}
