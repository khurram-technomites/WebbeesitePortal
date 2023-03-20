using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierOrderService : ISupplierOrderService
    {
        private readonly ISupplierOrderRepo _repo;
        public SupplierOrderService(ISupplierOrderRepo repo)
        {
            _repo = repo;
        }

        public async Task<SupplierOrder> AddSupplierOrderAsync(SupplierOrder Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<SupplierOrder>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<SupplierOrder>> GetAllByRestaurantAsync(long restaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects: "Supplier,SupplierOrderDetails");
        }


        public async Task<IEnumerable<SupplierOrder>> GetBySupplierIdAsync(long supplierId)
        {
            return await _repo.GetByIdAsync(x => x.SupplierId == supplierId);
        }

        public async Task<SupplierOrder> UpdateSupplierOrderAsync(SupplierOrder Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<IEnumerable<SupplierOrder>> GetByIdAsync(long Id)
        {
            return await _repo.GetAllAsync(x => x.Id == Id, ChildObjects: "Supplier , SupplierOrderDetails , SupplierOrderDetails.SupplierItem");
        }
        public async Task<IEnumerable<SupplierOrder>> GetAllBySupplierId(long supplierId)
        {
            return await _repo.GetAllAsync(x => x.SupplierId == supplierId);
        }
        public async Task<IEnumerable<SupplierOrder>> GetAllByRestaurantId(long RestaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == RestaurantId);
        }
        public async Task<SupplierOrder> ArchiveSupplierOrderAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<IEnumerable<SupplierOrder>> GetAllOrdersBySupplierAndStatus(long supplierID, string status)
        {
            if (supplierID > 0 && status != "All")
            {
                return await _repo.GetAllAsync(x => x.SupplierId == supplierID && x.Status == status, ChildObjects: "Restaurant , Supplier , SupplierOrderDetails");
            }
            else
            {
                return await _repo.GetAllAsync(x => x.SupplierId == supplierID, ChildObjects: "Restaurant , Supplier ,SupplierOrderDetails");

            }


        }
        public async Task<IEnumerable<SupplierOrder>> GetAllOrdersByDateAndStatus(long supplierId, string status, DateTime OrderRequiredDate, DateTime OrderRequiredDate2)
        {

            if (status != "All" && OrderRequiredDate.ToString() != "1/1/0001 12:00:00 AM" && OrderRequiredDate2.ToString() != "1/1/0001 12:00:00 AM")
            {
                return await _repo.GetAllAsync(x => x.SupplierId == supplierId && x.Status == status && x.OrderRequiredDate.Value >= OrderRequiredDate.Date && x.OrderRequiredDate.Value <= OrderRequiredDate2.Date, ChildObjects: "Restaurant , Supplier , SupplierOrderDetails");
            }
            else if (status != "All" && OrderRequiredDate.ToString() == "1/1/0001 12:00:00 AM" && OrderRequiredDate2.ToString() == "1/1/0001 12:00:00 AM")
            {
                return await _repo.GetAllAsync(x => x.SupplierId == supplierId && x.Status == status, ChildObjects: "Restaurant , Supplier , SupplierOrderDetails");
            }
            else if (status == "All" && OrderRequiredDate.ToString() == "1/1/0001 12:00:00 AM" && OrderRequiredDate2.ToString() == "1/1/0001 12:00:00 AM")
            {
                return await _repo.GetAllAsync(x => x.SupplierId == supplierId, ChildObjects: "Restaurant , Supplier ,SupplierOrderDetails");
            }
            else
            {
                return await _repo.GetAllAsync(x => x.SupplierId == supplierId && x.OrderRequiredDate.Value >= OrderRequiredDate.Date && x.OrderRequiredDate.Value <= OrderRequiredDate2.Date, ChildObjects: "Restaurant , Supplier ,SupplierOrderDetails");
            }


        }
        public async Task<IEnumerable<SupplierOrder>> GetAllRestaurantOrdersByStatus(long restaurantId, string status)
        {
            if (restaurantId > 0 && status != "All")
            {
                return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId && x.Status == status, ChildObjects: "Restaurant , Supplier , SupplierOrderDetails");
            }
            else
            {
                return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects: "Restaurant , Supplier ,SupplierOrderDetails");

            }


        }
        public async Task<long> GetAllOrdersCountAsync()
        {
            return await _repo.GetCount();
        }
        public async Task<long> GetAllOrdersBySupplierIDCountAsync(long supplierId)
        {
            return await _repo.GetCount(x => x.SupplierId == supplierId);
        }
        public async Task<long> GetAllOrdersCancelledStausCountAsync(long supplierId)
        {
            return await _repo.GetCount(x => x.Status == "Canceled" && x.SupplierId == supplierId);
        }
        public async Task<long> GetAllOrdersFoodReadyStausCountAsync(long supplierId)
        {
            return await _repo.GetCount(x => x.Status == "OrderReady" && x.SupplierId == supplierId);
        }
        public async Task<long> GetAllOrdersPreparingStausCountAsync(long supplierId)
        {
            return await _repo.GetCount(x => x.Status == "Processing" && x.SupplierId == supplierId);
        }
        public async Task<long> GetAllOrdersConfirmedStausCountAsync(long supplierId)
        {
            return await _repo.GetCount(x => x.Status == "Confirmed" && x.SupplierId == supplierId);
        }
        public async Task<long> GetAllOrdersOnTheWayStausCountAsync(long supplierId)
        {
            return await _repo.GetCount(x => x.Status == "OnTheWay" && x.SupplierId == supplierId);
        }
        public async Task<long> GetAllOrdersDeliveredStausCountAsync(long supplierId)
        {
            return await _repo.GetCount(x => x.Status == "Delivered" && x.SupplierId == supplierId);
        }
        public async Task<long> GetAllOrdersPendingStausCountAsync(long supplierId)
        {
            return await _repo.GetCount(x => x.Status == "Pending" && x.SupplierId == supplierId);
        }
    }
}
