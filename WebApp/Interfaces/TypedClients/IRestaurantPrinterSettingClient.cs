using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
	public interface IRestaurantPrinterSettingClient
	{
		Task<IEnumerable<RestaurantPrinterSettingDTO>> GetAllAsync();
		Task<RestaurantPrinterSettingDTO> GetAllByIdAsync(long Id);
		Task<IEnumerable<RestaurantPrinterSettingDTO>> GetAllByRestaurantIdAsync(long restaurantId);
		Task<RestaurantPrinterSettingDTO> GetByRestaurantBranchIdAsync(long restaurantBranchId);
		Task<RestaurantPrinterSettingDTO> AddRestaurantPrinterSettingAsync(RestaurantPrinterSettingDTO Entity);
		Task<RestaurantPrinterSettingDTO> UpdateRestaurantPrinterSettingAsync(RestaurantPrinterSettingDTO Entity);
		Task DeleteRestaurantPrinterSettingAsync(long printerSettingId);
		Task<RestaurantPrinterSettingDTO> ToggleActiveStatus(long RestaurantPrinterId);




	}
}
