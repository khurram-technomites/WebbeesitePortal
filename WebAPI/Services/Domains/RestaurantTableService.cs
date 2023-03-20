using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantTableService : IRestaurantTableService
    {
        private readonly IRestaurantTableRepo _repo;

        public RestaurantTableService(IRestaurantTableRepo repo, IMapper mapper)
        {
            _repo = repo;
        }

        public async Task<RestaurantTable> AddRestaurantTableAsync(RestaurantTable Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<RestaurantTable> ArchiveRestaurantTableAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<RestaurantTable> UpdateRestaurantTableAsync(RestaurantTable Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<IEnumerable<RestaurantTable>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<RestaurantTable> GetByIdAsync(long Id)
        {
            var result = await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Restaurant ,RestaurantBranch, RestaurantTableReservations, RestaurantTableReservations.Order");
            return result.FirstOrDefault();
        }
        public async Task<IEnumerable<RestaurantTable>> GetByRestaurantIdAsync(long RestaurantId)
        {
            var list = await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant ,RestaurantBranch, RestaurantTableReservations, RestaurantTableReservations.Order");
            return list;
        }
        public async Task<IEnumerable<RestaurantTable>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
        {
            var list = await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId && x.ActiveStatus == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "RestaurantBranch, RestaurantTableReservations, RestaurantTableReservations.Order");
            return list;
        }

        public async Task<List<RestaurantTableDTO>> GetReservedByRestaurantBranchIdAsync(long RestaurantBranchId, string Status)
        {
            var list = await _repo.GetReservedByRestaurantBranchID(RestaurantBranchId, Status);
            return list;
        }
    }
}
