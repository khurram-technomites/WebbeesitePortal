using HelperClasses.DTOs.Aggregators;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantUserLogManagementClient
    {
        Task<IEnumerable<RestaurantUserLogManagementDTO>> GetUserLogManagementByRestaurantIdAsync(long RestaurantId);

    }
}
