using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantRatingService : IRestaurantRatingService
    {
        private readonly IRestaurantRatingRepo _repo;

        public RestaurantRatingService(IRestaurantRatingRepo repo)
        {
            _repo = repo;
        }

        public async Task<RestaurantRating> AddRestaurantRatingAsync(RestaurantRating Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<IEnumerable<RestaurantRating>> GetAllRatingForRestaurantAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.OrderId == null, ChildObjects : "Restaurant , User");
        }
        public async Task<IEnumerable<RestaurantRating>> GetRestaurantRatingByIdAsync(long RatingId)
        {
            return await _repo.GetByIdAsync(x => x.Id == RatingId, ChildObjects: "Restaurant , User");
        }
        public async Task<IEnumerable<RestaurantRating>> GetAllRatingByStatusAsync(string Status , long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.Status == Status && x.RestaurantId == RestaurantId, ChildObjects: "Restaurant , User");
        }
        public async Task<IEnumerable<RestaurantRating>> GetRestaurantRatingByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant , User");
        }
        public async Task<RestaurantRating> UpdateRestaurantRatingAsync(RestaurantRating Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
