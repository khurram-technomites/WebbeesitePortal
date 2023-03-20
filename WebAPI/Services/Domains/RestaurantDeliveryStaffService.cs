using HelperClasses.Classes;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantDeliveryStaffService : IRestaurantDeliveryStaffService
    {
        private readonly IRestaurantDeliveryStaffRepo _repo;
        public RestaurantDeliveryStaffService(IRestaurantDeliveryStaffRepo repo)
        {
            _repo = repo;
        }

        public async Task<RestaurantDeliveryStaff> AddRestaurantDeliveryStaffAsync(RestaurantDeliveryStaff Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<RestaurantDeliveryStaff> ArchiveRestaurantDeliveryStaffAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<RestaurantDeliveryStaff>> GetAllRestaurantDeliveryStaffsAsync(long restaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects: "RestaurantBranch");
        }

        public async Task<IEnumerable<RestaurantDeliveryStaff>> GetRestaurantDeliveryStaffByBranchAsync(long BranchId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBranchId == BranchId && x.Status == Enum.GetName(typeof(Status), Status.Active));
        }

        public async Task<IEnumerable<RestaurantDeliveryStaff>> GetRestaurantDeliveryStaffByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Restaurant");
        }

        public async Task<IEnumerable<RestaurantDeliveryStaff>> GetRestaurantDeliveryStaffByImageAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Logo == Path);
        }

        public async Task<RestaurantDeliveryStaff> GetRestaurantDeliveryStaffByUserAsync(string UserId)
        {
            IEnumerable<RestaurantDeliveryStaff> list = await _repo.GetByIdAsync(x => x.UserId == UserId);
            return list.FirstOrDefault();
        }

        public async Task<RestaurantDeliveryStaff> UpdateRestaurantDeliveryStaffAsync(RestaurantDeliveryStaff Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }


    }
}
