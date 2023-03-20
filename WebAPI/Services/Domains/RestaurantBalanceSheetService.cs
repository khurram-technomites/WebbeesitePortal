using HelperClasses.DTOs.Restaurant;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class RestaurantBalanceSheetService : IRestaurantBalanceSheetService
	{
		private readonly IRestaurantBalanceSheetRepo _repo;
		public RestaurantBalanceSheetService(IRestaurantBalanceSheetRepo repo)
		{
			_repo = repo;
		}

		public async Task<IEnumerable<RestaurantBalanceSheet>> GetAllAsync()
		{
			return await _repo.GetAllAsync(ChildObjects: "RestaurantBranch, restaurantCashierStaff");
		}

		public async Task<RestaurantBalanceSheet> GetByIdAsync(long Id)
		{
			var list = await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "RestaurantBranch, restaurantCashierStaff");

			return list.FirstOrDefault();
		}

		//public async Task<RestaurantBalanceSheet> GetShiftDetails(RestaurantBalanceSheet balanceSheet)
		//{
		//	return await _repo.GetShiftDetails(balanceSheet);
		//}

		public async Task<IEnumerable<RestaurantBalanceSheet>> GetByRestaurantIdAsync(long RestaurantId)
		{
			return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "RestaurantBranch, restaurantCashierStaff");
		}
		public async Task<IEnumerable<RestaurantBalanceSheet>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
		{
			return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId, ChildObjects: "RestaurantBranch");
		}
		public async Task<IEnumerable<RestaurantBalanceSheet>> GetByRestaurantCashierStaffIdAsync(long CashierStaffId)
		{
			return await _repo.GetByIdAsync(x => x.RestaurantCashierStaffId == CashierStaffId, ChildObjects: "restaurantCashierStaff");
		}
		public async Task<IEnumerable<RestaurantBalanceSheet>> GetByRestaurantCashierStaffIdAsync(long CashierStaffId, string deviceId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantCashierStaffId == CashierStaffId && x.DeviceId == deviceId, ChildObjects: "restaurantCashierStaff");
		}
		public async Task<RestaurantBalanceSheetReportDTO> GetReportByRestaurantCashierStaffIdAsync(long CashierStaffId, long? Id)
		{
			return await _repo.GetReportyRestaurantCashierStaffIdAsync(CashierStaffId, Id);
		}

		public async Task<RestaurantBalanceSheet> AddRestaurantBalanceSheetAsync(RestaurantBalanceSheet Model)
		{
			return await _repo.InsertAsync(Model);
		}
		public async Task<RestaurantBalanceSheet> UpdateRestaurantBalanceSheetAsync(RestaurantBalanceSheet Model)
		{
			return await _repo.UpdateAsync(Model);
		}
		public async Task<RestaurantBalanceSheet> ArchiveRestaurantBalanceSheetAsync(long Id)
		{
			return await _repo.ArchiveAsync(Id);
		}


	}
}
