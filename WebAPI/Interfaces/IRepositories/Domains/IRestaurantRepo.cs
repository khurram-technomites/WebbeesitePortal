using HelperClasses.DTOs;
using HelperClasses.DTOs.Menu;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface IRestaurantRepo : IRepository<Restaurant>
    {
        IEnumerable<RestaurantCardResponseDTO> GetAllNearMe(RestaurantFilter Filter);
        IEnumerable<RestaurantCardResponseDTO> Trending(RestaurantFilter Filter);
        Task<IEnumerable<PopularCategories>> GetAllPopularCategoriesByRestaurantBranch(long BranchId);
        IEnumerable<LandingPageResponseDTO> GetRestaurantDetailsByOrigin(string Origin);
        Task<IEnumerable<LandingPageResponseDTO>> GetRestaurantBranchDetails(long BranchId, long? CustomerId);
        Task<IEnumerable<Restaurant>> GetRestaurantByOrigin(string Origin);
        object GetNearestBranch(decimal lat, decimal lng, long RestaurantId);
        Task<List<BranchMenuDTO>> GetBranchMenu(long BranchId);
        //Task<List<MenuPartnerDTO>> GetBranchMenuForPartner(long BranchId);
        Task<IEnumerable<MenuPartnerDTO>> GetBranchMenuForPartner(long BranchId);
        //Task<IEnumerable<MenuItem>> GetBranchMenu(long BranchId);
        Task<List<MenuItem>> GetBranchMenuItems(long BranchId);
    }
}
