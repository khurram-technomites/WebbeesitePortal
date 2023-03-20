using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantRatingClient
    {
        Task<IEnumerable<RestaurantRatingDTO>> GetRestaurantRatings();
        Task<RestaurantRatingDTO> ToggleActiveStatus(long RatingId , string status);
        Task<RestaurantRatingDTO> GetRestaurantRatingByID(long Id);
        Task<IEnumerable<RestaurantRatingDTO>> GetRestaurantRatingByStatus(string Status , long RestaurantId);
        Task<IEnumerable<RestaurantRatingDTO>> GetRestaurantRatingByRestaurantID(long Id);
        Task<RestaurantRatingDTO> Edit(RestaurantRatingDTO model);
    }
}
