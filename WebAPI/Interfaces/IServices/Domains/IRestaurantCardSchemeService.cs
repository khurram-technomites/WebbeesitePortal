using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantCardSchemeService
    {
        Task<IEnumerable<RestaurantCardScheme>> GetAllAsync();
        Task<IEnumerable<RestaurantCardScheme>> GetByIdAsync(long id);
        Task<IEnumerable<RestaurantCardScheme>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<RestaurantCardScheme>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<RestaurantCardScheme> AddRestaurantCardSchemeAsync(RestaurantCardScheme Model);
        Task<RestaurantCardScheme> UpdateRestaurantCardSchemeAsync(RestaurantCardScheme Model);
        Task<RestaurantCardScheme> ArchiveRestaurantCardSchemeAsync(long Id);
    }
}
