using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantSubscriberService
    {
        Task<RestaurantSubscriber> AddSubscriberAsync(RestaurantSubscriber Model);
        Task<IEnumerable<RestaurantSubscriber>> GetSubscribersByEmailAsync(string Email);
        Task<IEnumerable<RestaurantSubscriber>> GetSubscribersByRestaurantAsync(long RestaurantId);
        Task DeleteSubscriberAsync(long Id);
    }
}
