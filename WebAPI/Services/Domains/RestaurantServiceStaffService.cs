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
    public class RestaurantServiceStaffService : IRestaurantServiceStaffService
    {
        private readonly IRestaurantServiceStaffRepo _repo;

        public RestaurantServiceStaffService(IRestaurantServiceStaffRepo repo)
        {
            _repo = repo;
        }

        public async Task<RestaurantServiceStaff> AddRestaurantServiceStaffAsync(RestaurantServiceStaff Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<RestaurantServiceStaff> ArchiveRestaurantServiceStaffAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<RestaurantServiceStaff>> GetAllRestaurantServiceStaffAsync(PagingParameters Pagination)
        {
            return await _repo.GetAllAsync(Pagination: Pagination);
        }

        public async Task<IEnumerable<RestaurantServiceStaff>> GetAllRestaurantServiceStaffByBranchIdAsync(long RestaurantBranchId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantBranchId == RestaurantBranchId && x.Status == Enum.GetName(typeof(Status), Status.Active));
        }
        public async Task<RestaurantServiceStaff> GetAllRestaurantServiceStaffByIdAsync(long Id)
        {
            var details = await _repo.GetByIdAsync(x => x.Id == Id);
            return details.FirstOrDefault();
        }

        public async Task<IEnumerable<RestaurantServiceStaff>> GetAllRestaurantServiceStaffByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "RestaurantBranch");
        }

        public async Task<long> GetAllRestaurantServiceStaffsCountAsync()
        {
            return await _repo.GetCount();
        }
        public async Task<IEnumerable<RestaurantServiceStaff>> GetRestaurantServiceStaffByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "User");
        }


        public async Task<IEnumerable<RestaurantServiceStaff>> GetRestaurantServiceStaffByUserAsync(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.UserId == UserId, ChildObjects: "Restaurant, RestaurantBranch");
        }

        public async Task<RestaurantServiceStaff> UpdateRestaurantServiceStaffAsync(RestaurantServiceStaff Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }

    }
}
