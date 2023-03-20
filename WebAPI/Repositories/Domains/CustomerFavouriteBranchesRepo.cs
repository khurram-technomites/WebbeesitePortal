using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
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
    public class CustomerFavouriteBranchesRepo : Repository<CustomerFavouriteBranches>, ICustomerFavouriteBranchesRepo
    {
        private new readonly FougitoContext _context;
        public CustomerFavouriteBranchesRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {
            _context = context;
        }

        public IEnumerable<RestaurantCardResponseDTO> GetAllByCustomer(RestaurantFilter Filter, long CustomerId)
        {
            IQueryable<CustomerFavouriteBranches> result = _context.CustomerFavouriteBranches.Where(x=>x.CustomerId == CustomerId).Include(x => x.RestaurantBranch).ThenInclude(x=>x.BranchSchedules)
                .Include(x => x.RestaurantBranch).ThenInclude(x => x.Restaurant).ThenInclude(x => x.RestaurantRatings).Where(x => x.RestaurantBranch.Restaurant.Status == Enum.GetName(typeof(Status), Status.Active))
                .AsQueryable();

            IEnumerable<RestaurantCardResponseDTO> List = result.Include(x => x.RestaurantBranch.BranchSchedules).AsEnumerable().Select(x => new RestaurantCardResponseDTO
            {
                Id = x.BranchId,
                RestaurantId = x.RestaurantBranch.RestaurantId,
                DeliveryTime = x.RestaurantBranch.DeliveryMinutes,
                ServiceDistance = x.RestaurantBranch.ServiceDistance,
                Logo = !string.IsNullOrEmpty(x.RestaurantBranch.Restaurant.Logo) ? x.RestaurantBranch.Restaurant.Logo.Replace(" ", "%20") : "",
                NameAsPerTradeLicense = x.RestaurantBranch.Restaurant.NameAsPerTradeLicense,
                Address = x.RestaurantBranch.Address,
                Distance = DistanceHelper.DistanceTo((double)Filter.Latitude, (double)Filter.Longitude, (double)x.RestaurantBranch.Latitude, (double)x.RestaurantBranch.Longitude).ToString("n2") + " Km",
                OpeningTime = x.RestaurantBranch.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.RestaurantBranch.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().OpeningTime).ToShortTimeString() : null,
                ClosingTime = x.RestaurantBranch.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).Any() ?
                              (DateTime.Today + x.RestaurantBranch.BranchSchedules.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).FirstOrDefault().ClosingTime).ToShortTimeString() : null,
                AvgRating = x.RestaurantBranch.Restaurant.RestaurantRatings.Any() ? x.RestaurantBranch.Restaurant.RestaurantRatings.Average(x => x.Rating) : 0,
                RatingCount = x.RestaurantBranch.Restaurant.RestaurantRatings.Count,
                Slug = !string.IsNullOrEmpty(x.RestaurantBranch.Slug) ? x.RestaurantBranch.Slug : ""
            });

            return List.ToList();
        }
    }
}
