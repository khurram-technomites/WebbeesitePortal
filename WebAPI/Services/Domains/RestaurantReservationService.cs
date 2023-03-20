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
	public class RestaurantReservationService : IRestaurantReservationService
	{
		private readonly IRestaurantReservationRepo _repo;

		public RestaurantReservationService(IRestaurantReservationRepo repo, IMapper mapper)
		{
			_repo = repo;
		}

		public async Task<IEnumerable<RestaurantReservation>> GetAllAsync()
		{
			return await _repo.GetAllAsync();
		}

		public async Task<RestaurantReservation> GetByIdAsync(long Id)
		{
			var result = await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Restaurant, RestaurantBranch, RestaurantCashierStaff");
			return result.FirstOrDefault();
		}

		public async Task<RestaurantReservation> GetByContactAsync(string contact)
		{
			var result = await _repo.GetAllAsync(x => x.Contact == contact, ChildObjects: "", null, OrderExp: x => x.Id, true);
			return result.FirstOrDefault();
		}

		public async Task<IEnumerable<RestaurantReservation>> GetByRestaurantIdAsync(long RestaurantId)
		{
			var list = await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant, RestaurantBranch, RestaurantCashierStaff");
			return list/*.Where(x => x.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Reserved))*/;
		}

		public async Task<IEnumerable<RestaurantReservation>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
		{
			var list = await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId, ChildObjects: "RestaurantBranch, RestaurantCashierStaff");
			return list/*.Where(x => x.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Reserved))*/;
		}

		public async Task<IEnumerable<RestaurantReservation>> GetByStatusAndRestaurantBranchIdAsync(long RestaurantBranchId, string Status)
		{
			var list = await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId, ChildObjects: "RestaurantBranch, RestaurantCashierStaff");
			return list;
		}

		public async Task<RestaurantReservation> AddRestaurantReservationAsync(RestaurantReservation Model)
		{
			return await _repo.InsertAsync(Model);
		}

		public async Task<RestaurantReservation> ArchiveRestaurantReservationAsync(long Id)
		{
			return await _repo.ArchiveAsync(Id);
		}

		public async Task<RestaurantReservation> UpdateRestaurantReservationAsync(RestaurantReservation Model)
		{
			return await _repo.UpdateAsync(Model);
		}

	}
}
