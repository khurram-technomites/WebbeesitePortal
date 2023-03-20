using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantContentManagementClient
    {
        Task<IEnumerable<RestaurantContentManagementDTO>> GetRestaurantContentManagement();
        Task<RestaurantContentManagementDTO> Create(RestaurantContentManagementDTO model);
        Task<RestaurantContentManagementDTO> Update(RestaurantContentManagementDTO model);
        Task<RestaurantContentManagementDTO> Footer(RestaurantContentManagementDTO model);
        Task<IEnumerable<RestaurantContentManagementDTO>> GetRestaurantContentManagementByID(long Id);
        Task<RestaurantContentManagementDTO> GetRestaurantContentManagementByRestaurantId(long RestaurantId);
    }
}
