using HelperClasses.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantOrderClient
    {
        Task<IEnumerable<OrderDTO>> GetAllOrderByRestaurantAsync(long RestaurantId);
        Task<OrderDTO> GetOrderByIdAsync(long Id);
        Task<OrderDTO> UpdateStatusOrderAsync(OrderDTO Entity);
        Task<IEnumerable<OrderDTO>> GetAllOrderByRestaurantBranchAsync(long BranchId);
        Task<IEnumerable<OrderDTO>> GetAllOrdersByStatus(OrderDTO Entity);

    }
}
