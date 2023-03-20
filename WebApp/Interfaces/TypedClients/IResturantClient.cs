using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IResturantClient
    {
        Task<IEnumerable<RestaurantDTO>> GetResturant(PagingParameters model);
        Task<RestaurantDTO> GetResturantByID(long Id);
        Task<IEnumerable<RestaurantDTO>> GetRestaurantForDropDwonAsync(PagingParameters model);
        Task<IEnumerable<RestaurantImagesDTO>> GetRestaurantImagesAsync(long RestaurantId);
        Task<IEnumerable<RestaurantDTO>> GetRestaurantForDropDwonAssignAsync(PagingParameters model);
        Task<RestaurantDTO> Create(RestaurantDTO model);
        Task<RestaurantDTO> Edit(RestaurantDTO model);
        Task<RestaurantDTO> EditAboutUsImage(long Id);
        Task<RestaurantDTO> UnAssignSupplierCode(RestaurantDTO model);

        Task<RestaurantDTO> Delete(long Id);
    }
}
