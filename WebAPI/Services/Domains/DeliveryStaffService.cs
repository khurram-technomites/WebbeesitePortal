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
    public class DeliveryStaffService : IDeliveryStaffService
    {
        private readonly IDeliveryStaffRepo _repo;
        private readonly IRestaurantDeliveryStaffRepo _deliveryRepo;
        public DeliveryStaffService(IDeliveryStaffRepo repo , IRestaurantDeliveryStaffRepo deliveryRepo)
        {
            _repo = repo;
            _deliveryRepo = deliveryRepo;
        }

        public async Task<DeliveryStaff> AddDeliveryStaffAsync(DeliveryStaff Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<DeliveryStaff> ArchiveDeliveryStaffAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<DeliveryStaff>> GetAllDeliveryStaffsAsync(PagingParameters Pagination)
        {
            return await _repo.GetAllAsync(Pagination: Pagination,  OrderExp: x => x.Id , ChildObjects: "User");
        }
        public async Task<long> GetAllDeliveryStaffsCountAsync()
        {
            return await _repo.GetCount();
        }
        public async Task<long> GetAllRestaurantDeliveryStaffsCountAsync(long restaurantId)
        {
            return await _deliveryRepo.GetCount(x => x.RestaurantId == restaurantId);
        }
        public async Task<IEnumerable<DeliveryStaff>> GetDeliveryStaffByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id,  ChildObjects: "User");
        }

   
        public async Task<IEnumerable<DeliveryStaff>> GetDeliveryStaffByUserAsync(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.UserId == UserId);
        }

        public async Task<DeliveryStaff> UpdateDeliveryStaffAsync(DeliveryStaff Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
