using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantSubscriberService : IRestaurantSubscriberService
    {
        private readonly IRestaurantSubscriberRepo _repo;

        public RestaurantSubscriberService(IRestaurantSubscriberRepo repo)
        {
            _repo = repo;
        }
        public async Task<RestaurantSubscriber> AddSubscriberAsync(RestaurantSubscriber Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task DeleteSubscriberAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<RestaurantSubscriber>> GetSubscribersByEmailAsync(string Email)
        {
            return await _repo.GetByIdAsync(x => x.Email == Email);
        }

        public async Task<IEnumerable<RestaurantSubscriber>> GetSubscribersByRestaurantAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId);
        }
    }
}
