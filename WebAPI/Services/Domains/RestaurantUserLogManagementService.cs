using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class RestaurantUserLogManagementService : IRestaurantUserLogManagementService
	{
		private readonly IRestaurantUserLogManagementRepo _repo;
		public RestaurantUserLogManagementService(IRestaurantUserLogManagementRepo repo)
		{
			_repo = repo;
		}

		public async Task<IEnumerable<RestaurantUserLogManagement>> GetAllAsync()
		{
			return await _repo.GetAllAsync(ChildObjects: "User, RestaurantBranch");
		}
		public async Task<RestaurantUserLogManagement> GetByIdAsync(long Id)
		{
			IEnumerable<RestaurantUserLogManagement> list = await _repo.GetByIdAsync(x => x.Id == Id);
			return list.FirstOrDefault();
		}
		public async Task<IEnumerable<RestaurantUserLogManagement>> GetByRestaurantIdAsync(long RestaurantId)
		{
			return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "User, RestaurantBranch");
		}
		//public async Task<IEnumerable<RestaurantUserLogManagement>> GetByServiceStaffIdAsync(long ServiceStaffId)
		//{
		//	return await _repo.GetByIdAsync(x => x.ServiceStaffId == ServiceStaffId, ChildObjects: "ServiceStaff");
		//}
		public async Task<IEnumerable<RestaurantUserLogManagement>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
		{
			return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId, ChildObjects: "User, RestaurantBranch");
		}
		public async Task<RestaurantUserLogManagement> AddRestaurantUserLogManagementArgumentsAsync(DateTime? LoginTime, string DeviceID, string UserId, string UserType, long? UserDetailId, long RestaurantId, long RestaurantBranchId)
		{
			IEnumerable<RestaurantUserLogManagement> list = await _repo.GetAllAsync(x => x.UserId == UserId && x.Status == Enum.GetName(typeof(UserLogStatus), UserLogStatus.Login));
			RestaurantUserLogManagement userLog = list.LastOrDefault();
			if (userLog != null)
				return userLog;

			RestaurantUserLogManagement model = new()
			{
				LoginTime = LoginTime,
				DeviceID = DeviceID,
				UserId = UserId,
				UserType = UserType,
				UserDetailId = UserDetailId,
				RestaurantId = RestaurantId,
				RestaurantBranchId = RestaurantBranchId,
				Status = Enum.GetName(typeof(UserLogStatus), UserLogStatus.Login)
			};
			return await AddRestaurantUserLogManagementAsync(model);
		}
		public async Task<RestaurantUserLogManagement> AddRestaurantUserLogManagementAsync(RestaurantUserLogManagement Model)
		{
			return await _repo.InsertAsync(Model);
		}
		public async Task<RestaurantUserLogManagement> UpdateRestaurantUserLogManagementArgumentsAsync(DateTime? LogoutTime, string UserId, string UserType)
		{
			IEnumerable<RestaurantUserLogManagement> list = await _repo.GetAllAsync(x => x.UserId == UserId && x.UserType == UserType && x.Status == Enum.GetName(typeof(UserLogStatus), UserLogStatus.Login));

			if (list.Count() < 1)
				return null; 

			RestaurantUserLogManagement userLog = list.LastOrDefault();
			userLog.Status = Enum.GetName(typeof(UserLogStatus), UserLogStatus.Logout);
			userLog.LogoutTime = LogoutTime;

			return await UpdateRestaurantUserLogManagementAsync(userLog);
		}
		public async Task<RestaurantUserLogManagement> UpdateRestaurantUserLogManagementAsync(RestaurantUserLogManagement Model)
		{
			return await _repo.UpdateAsync(Model);
		}
		public async Task<RestaurantUserLogManagement> ArchiveRestaurantUserLogManagementAsync(long Id)
		{
			return await _repo.ArchiveAsync(Id);
		}

	}
}
