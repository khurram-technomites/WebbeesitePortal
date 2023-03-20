using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantBranchScheduleService
    {
        Task<IEnumerable<RestaurantBranchSchedule>> GetRestaurantBranchScheduleByBranch(long branchId);
        Task<RestaurantBranchSchedule> AddRestaurantBranchScheduleAsync(RestaurantBranchSchedule Model);
        Task<IEnumerable<RestaurantBranchSchedule>> GetRestaurantBranchScheduleById(long id);
        Task<RestaurantBranchSchedule> UpdateRestaurantBranchScheduleAsync(RestaurantBranchSchedule Model);
        Task ArchiveRestaurantBranchScheduleAsync(long Id);
        Task<IEnumerable<RestaurantBranchSchedule>> GetScheduleByDay(string day, TimeSpan openingTime, TimeSpan closingTime, long branchId, long Id = 0);
    }
}
