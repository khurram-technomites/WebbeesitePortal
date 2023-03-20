using HelperClasses.Classes;
using HelperClasses.DTOs.Garage;
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
    public class RequestRepo : Repository<SparePartRequest>, IRequestRepo
    {
        private readonly new FougitoContext _context;
        public RequestRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<IEnumerable<SparePartAvailableRequests>> GetAvailableRequestByCarMake(long CarMakeId)
        {
            return await _context.SparePartAvailableRequests
                //.Where(x => x.CarMake == CarMakeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetSparePartDealersByCarMake(long CarMakeId)
        {
            return await _context.SparePartAvailableRequests
                //.Where(x => x. == CarMakeId)
                .Select(x => x.UserId).ToListAsync();

        }
        public async Task<IEnumerable<string>> GetSparePartDealersBySparePartRequestId(long SparePartRquestId)
        {
            return await _context.SparePartAvailableRequests
                .Where(x => x.SparePartRequestId == SparePartRquestId)
                .Select(x => x.UserId).ToListAsync();

        }
        public async Task<IEnumerable<SparePartAvailableRequests>> GetAvailableRequestByStatus(string UserId, string Status)
        {
            return await _context.SparePartAvailableRequests
                  //.Where(x => x.CarMake == CarMakeId)
                  .ToListAsync();

        }
        public async Task<IEnumerable<SparePartRequest>> GetByUserAndFilter(long GarageId, SparePartRequestFilter Filter)
        {
            var result = _context.SparePartRequests.Where(x => x.GarageId == GarageId).Include(x => x.SparePartRequestImages).Include(x => x.CarModel).Include(x => x.CarMake).AsQueryable();
            if (Filter.Status != null)
            {
                result = result.Where(x => x.Status == Enum.GetName(typeof(StatusforGarageRequest), Filter.Status));
            }
          
            if (Filter.StartDate.HasValue && Filter.EndDate.HasValue)
                result = result.Where(x => x.CreationDate.Date >= Filter.StartDate.Value.Date && x.CreationDate.Date <= Filter.EndDate.Value.Date );

            if (Filter.SortDate == Sort.ASC)
                return await result.OrderBy(x => x.CreationDate.Date ).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize ).Take(Filter.Paging.PageSize).ToListAsync();
            else
                return await result.OrderByDescending(x => x.CreationDate.Date).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToListAsync();
        }
    }
}
