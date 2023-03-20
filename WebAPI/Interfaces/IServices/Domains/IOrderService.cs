using HelperClasses.DTOs;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Order.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
	public interface IOrderService
	{
		Task<Order> AddOrderAsync(Order Entity);
		Task<IEnumerable<Order>> GetAllOrdersByBranchPos(long branchId);
		Task<IEnumerable<Order>> GetAllPendingPaymentsByBranch(long branchId);
		Task<IEnumerable<Order>> GetAllDeliveryStaffCashByBranch(long branchId, DateTime openingDate, DateTime? closingDate);
		Task<Order> ArchiveOrderAsync(long Id);
		Task<IEnumerable<Order>> GetAllOrdersByBranchAndStatus(long branchId, string status);
		Task<IEnumerable<Order>> GetAllOrdersAsync(long? restaurantId, long? branchId);
		Task<IEnumerable<Order>> GetAllOrdersByRiderAsync(HelperClasses.DTOs.RestaurantDeliveryStaff.RestaurantDeliveryStaffOrderFilterDTO Model);
		Task<long> GetAllOrdersCountAsync();
		Task<long> GetAllOrdersCancelledStausCountAsync(long restaurantId = 0);
		Task<long> GetAllOrdersFoodReadyStausCountAsync(long restaurantId = 0);
		Task<long> GetAllOrdersPreparingStausCountAsync(long restaurantId = 0);
		Task<long> GetAllOrdersConfirmedStausCountAsync(long restaurantId = 0);
		Task<long> GetAllOrdersOnTheWayStausCountAsync(long restaurantId = 0);

		Task<long> GetAllOrdersDeliveredStausCountAsync(long restaurantId = 0);
		Task<long> GetAllOrdersPendingStausCountAsync(long restaurantId = 0);
		Task<long> GetAllOrdersByRestaurantIDCountAsync(long restaurantId = 0);
		Task<IEnumerable<Order>> GetAllOrdersByRestaurant(long restaurantId);
		Task<IEnumerable<Order>> GetAllOrdersByBranchAndStatus(long restaurantId, long branchId, string status);
		Task<IEnumerable<Order>> GetAllOrdersByStatus(long restaurantId, string Status);
		Task<IEnumerable<Order>> GetAllOrdersByBranch(long branchId);
		Task<IEnumerable<Order>> GetAllOrdersAsync();
		public IEnumerable<OrderShortDetailsDTO> GetAllByFilters(OrderFilter Filter);
		Task<IEnumerable<Order>> GetOrderByIdAsync(long Id);
		Task<IEnumerable<Order>> GetOrderByOrderNoAsync(string OrderNo);
		Task<IEnumerable<Order>> GetOrderByUserAsync(long CustomerId);
		Task<List<Order>> GetOrderByCashierStaffID(long cashierStaffId, DateTime openingDate, DateTime closingDate);
		Task<IEnumerable<Order>> GetOnGoingOrdersByUserAsync(long CustomerId, long RestaurantId);
		Task<IEnumerable<Order>> GetPastOrdersByUserAsync(long CustomerId, long RestaurantId);
		Task<Order> UpdateOrderAsync(Order Entity);
		Task<IEnumerable<OrderDTO>> GetAllOrdersByRestaurantForCustomer(long restaurantId);
		Task<List<OrderDetailOptionsDTO>> GetOptionsByOrderDetail(long OrderDetailId);
		Task<IEnumerable<Order>> GetAllOrdersByRestaurant(string Type = "TakeAway");
	}
}
