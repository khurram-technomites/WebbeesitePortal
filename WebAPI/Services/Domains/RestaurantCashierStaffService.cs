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
	public class RestaurantCashierStaffService : IRestaurantCashierStaffService
	{
		private readonly IRestaurantCashierStaffRepo _repo;
		public RestaurantCashierStaffService(IRestaurantCashierStaffRepo repo)
		{
			_repo = repo;
		}

		public async Task<RestaurantCashierStaff> AddRestaurantCashierStaffAsync(RestaurantCashierStaff Entity)
		{
			return await _repo.InsertAsync(Entity);
		}

		public async Task<RestaurantCashierStaff> ArchiveRestaurantCashierStaffAsync(long Id)
		{
			return await _repo.ArchiveAsync(Id);
		}

		public async Task<IEnumerable<RestaurantCashierStaff>> GetAllRestaurantCashierStaffAsync(PagingParameters Pagination)
		{
			return await _repo.GetAllAsync(Pagination: Pagination);
		}

		public async Task<IEnumerable<RestaurantCashierStaff>> GetAllRestaurantCashierStaffsAsync(long restaurantId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects: "Restaurant ,RestaurantBranch, User ");
		}

		public async Task<long> GetAllRestaurantCashierStaffsCountAsync(long restaurantId)
		{
			return await _repo.GetCount(x => x.RestaurantId == restaurantId);
		}

		public async Task<IEnumerable<RestaurantCashierStaff>> GetRestaurantCashierStaffByBranchAsync(long BranchId)
		{
			return await _repo.GetByIdAsync(x => x.RestaurantBranchId == BranchId && x.Status == Enum.GetName(typeof(Status), Status.Active));
		}

		public async Task<IEnumerable<RestaurantCashierStaff>> GetRestaurantCashierStaffByIdAsync(long Id)
		{
			return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Restaurant , RestaurantBranch");
		}

		public async Task<IEnumerable<RestaurantCashierStaff>> GetRestaurantCashierStaffByImageAsync(string Path)
		{
			return await _repo.GetByIdAsync(x => x.Logo == Path);
		}

		public async Task<RestaurantCashierStaff> GetRestaurantCashierStaffByUserAsync(string UserId, string deviceId = "")
		{
			IEnumerable<RestaurantCashierStaff> list = await _repo.GetByIdAsync(x => x.UserId == UserId, ChildObjects: "Restaurant ,RestaurantBranch, RestaurantBalanceSheets");
			var item = list.FirstOrDefault();
			//var balanceSheet = item.RestaurantBalanceSheets.LastOrDefault(x => x.DeviceId == deviceId);
			var balanceSheet = item.RestaurantBalanceSheets.LastOrDefault();

			item.RestaurantBalanceSheets = new List<RestaurantBalanceSheet>();

			if (balanceSheet != null && balanceSheet.Status == Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened))
				item.RestaurantBalanceSheets.Add(balanceSheet);

			return item;
		}

		public async Task<RestaurantCashierStaff> UpdateRestaurantCashierStaffAsync(RestaurantCashierStaff Entity)
		{
			return await _repo.UpdateAsync(Entity);
		}
	}
}
