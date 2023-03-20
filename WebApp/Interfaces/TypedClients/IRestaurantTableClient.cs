using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
	public interface IRestaurantTableClient
	{
		Task<IEnumerable<RestaurantTableDTO>> GetAllAsync();
		Task<RestaurantTableDTO> GetAllByIdAsync(long Id);
		Task<IEnumerable<RestaurantTableDTO>> GetAllByRestaurantIdAsync(long restaurantId);
		Task<IEnumerable<RestaurantTableDTO>> GetByRestaurantBranchIdAsync(long restaurantBranchId);
		Task<RestaurantTableDTO> AddRestaurantTableAsync(RestaurantTableDTO Entity);
		Task<RestaurantTableDTO> UpdateRestaurantTableAsync(RestaurantTableDTO Entity);
		Task DeleteRestaurantTableAsync(long Id);
		Task<RestaurantTableDTO> ToggleActiveStatus(long RestaurantTableId);
	}
}
