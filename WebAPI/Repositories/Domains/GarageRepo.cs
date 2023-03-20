using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageRepo : Repository<Garage>, IGarageRepo
    {
        private new readonly FougitoContext _context;

        public GarageRepo(FougitoContext context, ILoggerManager _logger) : base(context, _logger)
        {
            _context = context;
        }

        public IEnumerable<GarageCardResponseDTO> GetAllNearMe(GarageFilter Filter)
        {
            var result = _context.Garages.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active)).AsQueryable();

            if (!string.IsNullOrEmpty(Filter.Paging.Search))
                result = result.Where(x => EF.Functions.Like(x.NameAsPerTradeLicense, "%" + Filter.Paging.Search + "%"));

            if (Filter.CarMake is not 0)
                result = result.Include(x => x.GarageRepairSpecifications).Where(x => x.GarageRepairSpecifications.Any(x => x.CarMakeId == Filter.CarMake));


            var List = result.Include(x => x.GarageSchedules).Include(x => x.GarageRatings).AsEnumerable().Select(x => new GarageCardResponseDTO
            {
                Id = x.Id,
                Logo = !string.IsNullOrEmpty(x.Logo) ? x.Logo.Replace(" ", "%20") : "",
                NameAsPerTradeLicense = x.NameAsPerTradeLicense,
                Address = x.Address,
                Distance = DistanceHelper.DistanceTo((double)Filter.Latitude, (double)Filter.Longitude, (double)x.Latitude, (double)x.Longitude).ToString("n2") + " Km",
                OpeningTime = x.GarageSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.GarageSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().OpeningTime).ToShortTimeString() : null,
                ClosingTime = x.GarageSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.GarageSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().ClosingTime).ToShortTimeString() : null,
                AvgRating = x.GarageRatings.Any() ? x.GarageRatings.Average(x => x.Rating) : 0,
                RatingCount = x.GarageRatings.Count,
                Slug = x.Slug,
                ThumbnailImage = !string.IsNullOrEmpty(x.ThumbnailImage) ? x.ThumbnailImage.Replace(" ", "%20") : "",
            });

            //Switch case expression
            IEnumerable<GarageCardResponseDTO> Result = Filter.SortBy switch
            {
                1 => List.OrderBy(x => Convert.ToDouble(x.Distance.Replace(" Km", ""))).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                2 => List.OrderByDescending(x => x.Id).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                3 => List.OrderByDescending(x => x.AvgRating).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                _ => List.Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
            };
            return Result;
        }
    }
}
