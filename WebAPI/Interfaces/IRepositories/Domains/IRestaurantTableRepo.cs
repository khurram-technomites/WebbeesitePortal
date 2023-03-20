using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface IRestaurantTableRepo:IRepository<RestaurantTable>
    {
        Task<List<RestaurantTableDTO>> GetReservedByRestaurantBranchID(long RestaurantBranchId, string Status);
    }
}
