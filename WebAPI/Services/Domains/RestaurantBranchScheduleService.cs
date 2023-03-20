using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantBranchScheduleService : IRestaurantBranchScheduleService
    {
        private readonly IRestaurantBranchScheduleRepo _repo;

        public RestaurantBranchScheduleService(IRestaurantBranchScheduleRepo repo)
        {
            _repo = repo;
        }

        public async Task<RestaurantBranchSchedule> AddRestaurantBranchScheduleAsync(RestaurantBranchSchedule Model)
        {
            return await _repo.InsertAsync(Model);    
        }

        public async Task ArchiveRestaurantBranchScheduleAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<RestaurantBranchSchedule>> GetRestaurantBranchScheduleByBranch(long branchId)
        {
            var result = await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId);
            return result;
        }

        public async Task<IEnumerable<RestaurantBranchSchedule>> GetRestaurantBranchScheduleById(long id)
        {
            return await _repo.GetByIdAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<RestaurantBranchSchedule>> GetScheduleByDay(string day, TimeSpan openingTime , TimeSpan closingTime , long branchId , long Id = 0)
        {
            return await _repo.GetAllAsync(x => x.Day == day && x.OpeningTime == openingTime && x.ClosingTime == closingTime && x.RestaurantBranchId == branchId && x.Id != Id);
        }

        public async Task<RestaurantBranchSchedule> UpdateRestaurantBranchScheduleAsync(RestaurantBranchSchedule Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
