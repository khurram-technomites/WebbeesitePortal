using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Garage.Filter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartRequestQuoteRepo : Repository<SparePartRequestQuote>, ISparePartRequestQuoteRepo
    {
        private readonly new FougitoContext _context;
        public SparePartRequestQuoteRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<IEnumerable<SparePartAvailableRequests>> GetByUserAndFilter(string UserId, SparePartQuoteFilter Filter)
        {
            var result = _context.SparePartAvailableRequests.Where(x => x.UserId == UserId && x.Status == Enum.GetName(typeof(StatusforGarageRequest), Filter.Status )).OrderByDescending(x=>x.CreationDate).AsQueryable();

            if (Filter.StartDate.HasValue && Filter.EndDate.HasValue)
                result = result.Where(x => x.CreationDate.Date >= Filter.StartDate.Value.Date && x.CreationDate.Date <= Filter.EndDate.Value.Date).OrderByDescending(x=>x.CreationDate);

            if (Filter.SortDate == Sort.ASC)
                return await result.OrderBy(x => x.CreationDate.Date).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToListAsync();
            else
                return await result.OrderByDescending(x => x.CreationDate.Date).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToListAsync();
        }
        public async Task<IEnumerable<SparePartRequestQuote>> GetByQuoteFilter(long SparePartDealerId, SparePartQuoteFilter Filter)
        {
            //var result = _context.SparePartRequestQuotes.Where(x => x.SparePartsDealerId == SparePartDealerId).Include(x => x.SparePartRequest).Include(x => x.SparePartsDealer).AsQueryable();

            //if (Filter.StartDate.HasValue && Filter.EndDate.HasValue)
            //    result = result.Where(x => x.CreationDate.Date >= Filter.StartDate.Value.Date && x.CreationDate.Date <= Filter.EndDate.Value.Date);
            //   return await result.OrderByDescending(x => x.CreationDate.Date).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToListAsync();

            var result = _context.SparePartRequestQuotes.Where(x => x.SparePartsDealerId == SparePartDealerId).Include(x => x.SparePartRequest).Include(x => x.SparePartsDealer).OrderByDescending(x => x.CreationDate.Date).AsQueryable();
            if (Filter.Status != null)
            {
                result = result.Where(x => x.Status == Enum.GetName(typeof(StatusforGarageRequest), Filter.Status)).OrderByDescending(x => x.CreationDate.Date);
            }
            if (Filter.StartDate.HasValue && Filter.EndDate.HasValue)
                result = result.Where(x => x.CreationDate.Date >= Filter.StartDate.Value.Date && x.CreationDate.Date <= Filter.EndDate.Value.Date).OrderByDescending(x => x.CreationDate.Date);

            if (Filter.SortDate == Sort.ASC)
                return await result.OrderBy(x => x.CreationDate.Date).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToListAsync();
            else
                return await result.OrderByDescending(x => x.CreationDate.Date).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToListAsync();

        }
    }
}