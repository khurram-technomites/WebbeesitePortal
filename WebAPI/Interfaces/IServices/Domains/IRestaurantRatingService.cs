using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantRatingService
    {
        Task<RestaurantRating> AddRestaurantRatingAsync(RestaurantRating Model);
        Task<IEnumerable<RestaurantRating>> GetAllRatingForRestaurantAsync();
        Task<IEnumerable<RestaurantRating>> GetAllRatingByStatusAsync(string Status , long RestaurantId);
        Task<IEnumerable<RestaurantRating>> GetRestaurantRatingByIdAsync(long RatingId);
        Task<IEnumerable<RestaurantRating>> GetRestaurantRatingByRestaurantIdAsync(long RatingId);
        Task<RestaurantRating> UpdateRestaurantRatingAsync(RestaurantRating Entity);
    }
}
