using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierOrderService
    {
        Task<IEnumerable<SupplierOrder>> GetAllAsync();
        Task<IEnumerable<SupplierOrder>> GetByIdAsync(long Id);
        Task<IEnumerable<SupplierOrder>> GetAllByRestaurantAsync(long restaurantId);
        Task<IEnumerable<SupplierOrder>> GetAllByRestaurantId(long RestaurantId);
        Task<IEnumerable<SupplierOrder>> GetAllBySupplierId(long SupplierId);
        Task<SupplierOrder> AddSupplierOrderAsync(SupplierOrder Model);
        Task<SupplierOrder> UpdateSupplierOrderAsync(SupplierOrder Model);
        Task<SupplierOrder> ArchiveSupplierOrderAsync(long Id);
        Task<IEnumerable<SupplierOrder>> GetAllOrdersBySupplierAndStatus(long supplierId, string status);
        Task<IEnumerable<SupplierOrder>> GetAllOrdersByDateAndStatus(long supplierId ,string status , DateTime OrderRequiredDate , DateTime OrderRequiredDate2);
        Task<IEnumerable<SupplierOrder>> GetAllRestaurantOrdersByStatus(long restaurantId, string status);
        Task<long> GetAllOrdersCountAsync();
        Task<long> GetAllOrdersCancelledStausCountAsync(long supplierId = 0);
        Task<long> GetAllOrdersFoodReadyStausCountAsync(long supplierId = 0);
        Task<long> GetAllOrdersPreparingStausCountAsync(long supplierId = 0);
        Task<long> GetAllOrdersConfirmedStausCountAsync(long supplierId = 0);
        Task<long> GetAllOrdersOnTheWayStausCountAsync(long supplierId = 0);

        Task<long> GetAllOrdersDeliveredStausCountAsync(long supplierId = 0);
        Task<long> GetAllOrdersPendingStausCountAsync(long supplierId = 0);
        Task<long> GetAllOrdersBySupplierIDCountAsync(long supplierId = 0);

    }
}
