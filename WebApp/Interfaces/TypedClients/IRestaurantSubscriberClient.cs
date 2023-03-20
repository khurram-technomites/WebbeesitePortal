using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantSubscriberClient
    {
        Task<IEnumerable<RestaurantSubcriberViewModel>> GetByRestaurant(long RestaurantId);
        Task Delete(long Id);

        Task SendMessage(string Email, string Body);
    }
}
