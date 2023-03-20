using HelperClasses.DTOs;
using HelperClasses.DTOs.Menu;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantService 
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<long> GetAllRestaurantsCountAsync();
        IEnumerable<RestaurantCardResponseDTO> GetAllRestaurantsNearMe(RestaurantFilter Filter);
        IEnumerable<RestaurantCardResponseDTO> GetTrending(RestaurantFilter Filter);
        Task<IEnumerable<Restaurant>> GetRestaurantForDropDwonAsync();
        Task<IEnumerable<Restaurant>> GetRestaurantForDropDwonAssignAsync();
        Task<IEnumerable<Restaurant>> GetRestaurantByIdAsync(long? Id);
        Task<IEnumerable<Restaurant>> GetRestaurantBySlugAsync(string Slug);
        Task<IEnumerable<Restaurant>> GetRestaurantByUserAsync(string UserId);
        Task<Restaurant> AddRestaurantAsync(Restaurant Entity);
        Task<Restaurant> UpdateRestaurantAsync(Restaurant Entity);
        Task<Restaurant> ArchiveRestaurantAsync(long Id);
        Task<IEnumerable<PopularCategories>> GetPopularCategoriesByBranch(long BranchId);
        IEnumerable<LandingPageResponseDTO> GetRestaurantDetailsByOrigin(string Origin);
        Task<IEnumerable<LandingPageResponseDTO>> GetRestaurantBranchDetails(long BranchId, long? CustomerId);
        Task<IEnumerable<Restaurant>> GetRestaurantByOrigin(string Origin);
        object GetNearestRestaurant(long restaurantId, decimal lat, decimal lng);
        //Task<IEnumerable<MenuItem>> GetBranchMenu(long BranchId);
        Task<List<BranchMenuDTO>> GetBranchMenu(long BranchId);
        Task<IEnumerable<MenuPartnerDTO>> GetBranchMenuForPartner(long BranchId);
    }
}
