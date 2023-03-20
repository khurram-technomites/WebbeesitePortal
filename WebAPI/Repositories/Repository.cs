using HelperClasses.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private protected readonly FougitoContext _context;
        private readonly ILoggerManager _logger;

        public Repository(FougitoContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<T> ArchiveAsync(long Id)
        {
            var Entity = await _context.Set<T>().FindAsync(Id);

            if (Entity is not null)
            {
                Type EntityType = typeof(T);
                PropertyInfo ArchiveDate = EntityType.GetProperty("ArchivedDate");
                ArchiveDate.SetValue(Entity, DateTime.UtcNow.ToDubaiDateTime());

                _context.Update(Entity);
                await _context.SaveChangesAsync();
            }
            return Entity;
        }

        public async Task DeleteAsync(string Id)
        {
            var Entity = await _context.Set<T>().FindAsync(Id);
            _context.Set<T>().Remove(Entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long Id)
        {
            var Entity = await _context.Set<T>().FindAsync(Id);
            _context.Set<T>().Remove(Entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> List)
        {
            _context.Set<IEnumerable<T>>().RemoveRange(List);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> Expression, string ChildObjects = "", PagingParameters Pagination = null,
            Expression<Func<T, object>> OrderExp = null, bool IsOrderDescending = false)
        {
            try
            {
                var result = _context.Set<T>().Where(Expression);

                if (!String.IsNullOrEmpty(ChildObjects))
                {
                    string[] Childern = ChildObjects.Split(",");

                    foreach (string Child in Childern)
                        result = result.Include(Child.Trim());
                }

                if (OrderExp is not null)
                    result = result.OrderBy(OrderExp);
                if (OrderExp is not null && IsOrderDescending)
                    result = result.OrderByDescending(OrderExp);

                if (Pagination is null)
                {
                    return await result.ToListAsync();
                }

                return await result.Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
                                        .Take(Pagination.PageSize)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetAllAsync)} could not be Fetched: {ex.Message}, InnerException: {ex.InnerException}");
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }



        public async Task<IEnumerable<T>> GetAllAsync(string ChildObjects = "", PagingParameters Pagination = null, Expression<Func<T, object>> OrderExp = null)
        {
            try
            {
                var result = _context.Set<T>().AsQueryable();

                if (!String.IsNullOrEmpty(ChildObjects))
                {
                    string[] Childern = ChildObjects.Split(",");

                    foreach (string Child in Childern)
                        result = result.Include(Child.Trim());
                }

                if (OrderExp is not null)
                    result = result.OrderByDescending(OrderExp);

                if (Pagination is null)
                {
                    return await result.ToListAsync();
                }

                return await result.Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
                                                            .Take(Pagination.PageSize)
                                                            .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetAllAsync)} could not be Fetched: {ex.Message}, InnerException: {ex.InnerException}");
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<IEnumerable<T>> GetByIdAsync(Expression<Func<T, bool>> Expression, string ChildObjects = "", PagingParameters Pagination = null,
            Expression<Func<T, object>> OrderExp = null, bool IsOrderByDescending = false)
        {
            try
            {
                var result = _context.Set<T>().Where(Expression);

                if (!String.IsNullOrEmpty(ChildObjects))
                {
                    string[] Childern = ChildObjects.Split(",");

                    foreach (string Child in Childern)
                        result = result.Include(Child.Trim());
                }

                if (OrderExp is not null && !IsOrderByDescending)
                    result = result.OrderBy(OrderExp);

                if (OrderExp is not null && IsOrderByDescending)
                    result = result.OrderByDescending(OrderExp);

                if (Pagination is null)
                {
                    return await result.ToListAsync();
                }

                return await result.Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
                                                            .Take(Pagination.PageSize)
                                                            .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetByIdAsync)} could not be saved: {ex.Message}, InnerException: {ex.InnerException}");
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<long> GetCount(Expression<Func<T, bool>> Expression = null, string ChildObjects = "")
        {
            var result = _context.Set<T>().AsQueryable();

            if (!String.IsNullOrEmpty(ChildObjects))
            {
                string[] Childern = ChildObjects.Split(",");

                foreach (string Child in Childern)
                    result = result.Include(Child.Trim());
            }

            if (Expression is null)
                return await result.LongCountAsync();

            return await result.Where(Expression).LongCountAsync();
        }

        public async Task<IEnumerable<T>> GetGenericAsync(string Property, string value)
        {
            return await _context.Set<T>().Where(i => i.GetType().GetProperty(Property).GetValue(null).Equals(value)).ToListAsync();
        }

        public async Task<T> InsertAsync(T Entity)
        {
            string ImagePath = string.Empty;
            if (Entity == null)
            {
                _logger.LogWarning($"{nameof(InsertAsync)} Entity must not be null");
                throw new ArgumentNullException($"{nameof(InsertAsync)} Entity must not be null");
            }

            try
            {
                await _context.AddAsync(Entity);
                await _context.SaveChangesAsync();

                return Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Entity)} could not be saved: {ex.Message}, InnerException: {ex.InnerException}");
                throw new Exception($"{nameof(Entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> Entities)
        {
            if (Entities is null)
            {
                throw new ArgumentNullException($"{nameof(Entities)}: Entities must not be null");
            }

            try
            {
                await _context.AddRangeAsync(Entities);
                await _context.SaveChangesAsync();

                return Entities;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Entities)} could not be saved: {ex.Message}, InnerException: {ex.InnerException}");
                throw new Exception($"{nameof(Entities)} could not be saved: {ex.Message}");
            }
        }

        public async Task<T> UpdateAsync(T Entity)
        {
            if (Entity == null)
            {
                _logger.LogWarning($"{nameof(UpdateAsync)} Entity must not be null");
                throw new ArgumentNullException($"{nameof(UpdateAsync)} Entity must not be null");
            }

            try
            {
                _context.Update(Entity);
                await _context.SaveChangesAsync();

                return Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Entity)} could not be saved: {ex.Message}, InnerException: {ex.InnerException}");
                throw new Exception($"{nameof(Entity)} could not be updated: {ex.Message}");
            }
        }

        public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> Entities)
        {
            if (Entities is null)
            {
                throw new ArgumentNullException($"{nameof(Entities)}: Entities must not be null");
            }

            try
            {
                _context.UpdateRange(Entities);
                await _context.SaveChangesAsync();

                return Entities;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Entities)} could not be saved: {ex.Message}, InnerException: {ex.InnerException}");
                throw new Exception($"{nameof(Entities)} could not update Entities: {ex.Message}");
            }
        }
    }
}
