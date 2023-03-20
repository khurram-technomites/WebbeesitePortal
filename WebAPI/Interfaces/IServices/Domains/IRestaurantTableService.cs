using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
	public interface IRestaurantTableService
	{
		Task<IEnumerable<RestaurantTable>> GetAllAsync();
		Task<RestaurantTable> GetByIdAsync(long id);
		Task<IEnumerable<RestaurantTable>> GetByRestaurantIdAsync(long RestaurantId);
		Task<IEnumerable<RestaurantTable>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
		Task<List<RestaurantTableDTO>> GetReservedByRestaurantBranchIdAsync(long RestaurantBranchId, string Status);
		Task<RestaurantTable> AddRestaurantTableAsync(RestaurantTable Model);
		Task<RestaurantTable> UpdateRestaurantTableAsync(RestaurantTable Model);
		Task<RestaurantTable> ArchiveRestaurantTableAsync(long Id);
	}
}
