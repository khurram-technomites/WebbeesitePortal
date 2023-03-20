using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierOrderClient
    {
        //Task<IEnumerable<SupplierOrderDTO>> GetAllOrderByRestaurantAsync(long RestaurantId);
        Task<IEnumerable<SupplierOrderDTO>> GetOrderBySupplierIdAsync(long Id);
        Task<IEnumerable<SupplierOrderDTO>> GetAllSupplierOrderByRestaurantAsync(long RestaurantId);
        Task<IEnumerable<SupplierOrderDTO>> GetOrderByIdAsync(long Id);
        Task<SupplierOrderDTO> UpdateStatusOrderAsync(SupplierOrderDTO Entity);
        Task<IEnumerable<SupplierOrderDTO>> GetAllOrderByRestaurantBranchAsync(long BranchId);
        Task<IEnumerable<SupplierOrderDTO>> GetAllOrdersByStatus(SupplierOrderDTO Entity);
        Task<IEnumerable<SupplierOrderDTO>> GetAllOrdersByStatusandDate(SupplierOrderDTO Entity);
        Task<string> PlaceOrder(SupplierOrderPlacementDTO order);
        Task<int> Paid(string PaymentId, long OrderId);
        Task<IEnumerable<SupplierOrderDTO>> GetAllRestaurantOrdersByStatus( SupplierOrderDTO Entity);


    }
}
