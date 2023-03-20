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
    public class ServiceStaffService : IServiceStaffService
    {
        private readonly IServiceStaffRepo _repo;
        private readonly IRestaurantServiceStaffRepo _serviceRepo;

        public ServiceStaffService(IServiceStaffRepo repo , IRestaurantServiceStaffRepo serviceRepo)
        {
            _repo = repo;
            _serviceRepo = serviceRepo;
        }

        public async Task<ServiceStaff> AddServiceStaffAsync(ServiceStaff Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<ServiceStaff> ArchiveServiceStaffAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<ServiceStaff>> GetAllServiceStaffAsync(PagingParameters Pagination)
        {
            return await _repo.GetAllAsync(Pagination: Pagination);
        }
        public async Task<long> GetAllServiceStaffsCountAsync()
        {
            return await _repo.GetCount();
        }
        public async Task<long> GetAllServiceStaffsCountByRestaurantIdAsync(long restaurantId)
        {
            return await _serviceRepo.GetCount(x => x.RestaurantId == restaurantId);
        }
        public async Task<IEnumerable<ServiceStaff>> GetServiceStaffByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id , ChildObjects : "User");
        }

        public async Task<IEnumerable<ServiceStaff>> GetServiceStaffByUserAsync(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.UserId == UserId);
        }

        public async Task<ServiceStaff> UpdateServiceStaffAsync(ServiceStaff Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
