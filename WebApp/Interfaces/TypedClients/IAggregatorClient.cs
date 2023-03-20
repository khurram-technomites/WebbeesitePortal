using HelperClasses.DTOs;
using HelperClasses.DTOs.Aggregators;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
	public interface IAggregatorClient
	{
		Task<IEnumerable<AggregatorDTO>> GetAllAggregatorAsync();
		Task<IEnumerable<AggregatorDTO>> GetAggregatorByIdAsync(long Id);
		Task<IEnumerable<AggregatorDTO>> GetAggregatorByRestaurantIdAsync(long RestaurantId);
		//Task<AggregatorDTO> GetRestaurantBranchIdAsync(long RestaurantBranchId);
		Task<AggregatorDTO> AddAggregatorAsync(AggregatorDTO Entity);
		Task<AggregatorDTO> UpdateAggregatorAsync(AggregatorDTO Entity);
		Task DeleteAggregatorAsync(long CarMakeId);
		Task<AggregatorDTO> ToggleActiveStatus(long Id);

	}
}
