using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantClient
    {
        Task<RestaurantDTO> GetById(long Id);
        Task<IEnumerable<RestaurantDTO>> GetAll();
        Task<RestaurantDTO> Create(RestaurantDTO model);
        Task<IEnumerable<RestaurantImagesDTO>> GetRestaurantImagesAsync(long RestaurantId);
        Task<RestaurantDTO> Edit(RestaurantDTO model);
        Task<RestaurantDTO> Delete(long Id);
        Task<RestaurantDTO> ToggleActiveStatus(long Id);
        Task<RestaurantImagesDTO> DeleteImage(long Id);
    }
}
