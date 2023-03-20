using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IRepositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> Expression, string ChildObjects = "", PagingParameters Pagination = null, 
            Expression<Func<T, object>> OrderExp = null, bool IsOrderDescending = false);
        Task<IEnumerable<T>> GetAllAsync(string ChildObjects = "", PagingParameters Pagination = null, Expression<Func<T, object>> OrderExp = null);
        Task<IEnumerable<T>> GetByIdAsync(Expression<Func<T, bool>> Expression, string ChildObjects = "", PagingParameters Pagination = null,
            Expression<Func<T, object>> OrderExp = null, bool IsOrderByDescending = false);
        Task<long> GetCount(Expression<Func<T, bool>> Expression = null, string ChildObjects = "");
        Task<IEnumerable<T>> GetGenericAsync(string Property, string value);
        Task<T> InsertAsync(T Entity);
        Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> Entities);
        Task<T> UpdateAsync(T Entity);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> Entities);
        Task<T> ArchiveAsync(long Id);
        Task DeleteAsync(string Id);
        Task DeleteAsync(long Id);
        Task DeleteRangeAsync(IEnumerable<T> List);
    }
}
