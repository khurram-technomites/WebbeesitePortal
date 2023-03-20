using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Order.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class OrderService : IOrderService
	{

		private readonly IOrderRepo _repo;
		private readonly IMapper _mapper;
		public OrderService(IOrderRepo repo, IMapper mapper)
		{
			_repo = repo;
			_mapper = mapper;
		}

		public async Task<Order> AddOrderAsync(Order Entity)
		{
			if (Entity.CustomerId == 0)
				Entity.CustomerId = null;


			return await _repo.InsertAsync(Entity);
		}

		public async Task<Order> ArchiveOrderAsync(long Id)
		{
			return await _repo.ArchiveAsync(Id);
		}

		public async Task<IEnumerable<Order>> GetAllOrdersAsync(long? restaurantId, long? branchId)
		{

			return await _repo.GetAllAsync(x => (!restaurantId.HasValue || x.RestaurantBranch.RestaurantId == restaurantId) &&
											(!branchId.HasValue || x.RestaurantBranchId == branchId));

		}

		public async Task<IEnumerable<Order>> GetAllOrdersByRestaurant(long restaurantId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects: "RestaurantBranch , Restaurant");
		}

		public async Task<IEnumerable<Order>> GetAllOrdersByRestaurant(string Type)
		{
			return await _repo.GetAllAsync(x => x.Type == Type, ChildObjects: "RestaurantBranch , Restaurant , OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item");
		}

		public async Task<IEnumerable<OrderDTO>> GetAllOrdersByRestaurantForCustomer(long restaurantId)
		{
			return _mapper.Map<IEnumerable<OrderDTO>>(await _repo.GetOrderByCustomerID(restaurantId));
		}
		public async Task<IEnumerable<Order>> GetAllOrdersByBranch(long branchId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId, ChildObjects: "RestaurantBranch , Restaurant , OrderDetails ,  OrderDetails.MenuItems");
		}

		public async Task<IEnumerable<Order>> GetAllOrdersByBranchPos(long branchId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId && x.Origin == Enum.GetName(typeof(OrderOrigin), OrderOrigin.POS), ChildObjects: " OrderDetails, OrderDetails.MenuItems, OrderDetails.OrderDetailOptionValues,Customer , Customer.CustomerAddresses, RestaurantTable, DeliveryStaff, RestaurantWaiter ");
		}

		public async Task<IEnumerable<Order>> GetAllPendingPaymentsByBranch(long branchId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId
			&& x.IsPaid != true
			&& (x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Pending)
			|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed)
			|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Preparing)
			|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady))
			&& ((x.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.DineIn))
			|| (x.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Pickup))
			|| (x.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Delivery)))
			);
		}

		public async Task<IEnumerable<Order>> GetAllDeliveryStaffCashByBranch(long branchId, DateTime openingDate, DateTime? closingDate)
		{
			return await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId
			&& (x.CreationDate >= openingDate && (closingDate == null || x.CreationDate <= closingDate))
			&& ((x.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Delivery)))
			&& ((x.DeliveryStaffCash > 0))
			, ChildObjects: "DeliveryStaff", OrderExp: x => x.Id, IsOrderDescending: true);
		}

		public async Task<IEnumerable<Order>> GetAllOrdersByBranchAndStatus(long branchId, string type)
		{
			return await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId && x.DeliveryType == type && (x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed) ||
			x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Preparing) ||
			x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady)),
			 ChildObjects: "OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item, OrderDetails.OrderDetailOptionValues");
		}

		public async Task<IEnumerable<Order>> GetAllOrdersByBranchAndStatus(long restaurantId, long branchId, string status)
		{
			if (branchId == 0 && status == "All")
			{
				return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects: "RestaurantBranch , Restaurant");
			}

			else if (branchId > 0 && status != "All")
			{
				return await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId && x.Status == status, ChildObjects: "RestaurantBranch , Restaurant");
			}

			else if (branchId > 0 && status == "All")
			{
				return await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId, ChildObjects: "RestaurantBranch , Restaurant");
			}


			else if (branchId == 0 && status != "All")
			{
				return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId && x.Status == status, ChildObjects: "RestaurantBranch , Restaurant");
			}

			else
			{
				return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId && x.Status == status, ChildObjects: "RestaurantBranch , Restaurant");
			}


		}

		public async Task<IEnumerable<Order>> GetAllOrdersByStatus(long restaurantId, string Status)
		{
			return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId && x.Status == Status, ChildObjects: "RestaurantBranch , Restaurant");
		}

		public async Task<long> GetAllOrdersCountAsync()
		{
			return await _repo.GetCount();
		}
		public async Task<long> GetAllOrdersByRestaurantIDCountAsync(long restaurantId)
		{
			return await _repo.GetCount(x => x.RestaurantId == restaurantId);
		}

		//public async Task<bool> DeleteAllDetails(long Id)
		//{
		//    IEnumerable<Order> orders = await GetOrderByIdAsync(Id);
		//    Order order = orders.FirstOrDefault();
		//    foreach (var details in Detailsz )
		//    {

		//    }
		//}

		public async Task<long> GetAllOrdersCancelledStausCountAsync(long restaurantId)
		{
			return await _repo.GetCount(x => x.Status == "Canceled" && x.RestaurantId == restaurantId);
		}
		public async Task<long> GetAllOrdersFoodReadyStausCountAsync(long restaurantId)
		{
			return await _repo.GetCount(x => x.Status == "FoodReady" && x.RestaurantId == restaurantId);
		}
		public async Task<long> GetAllOrdersPreparingStausCountAsync(long restaurantId)
		{
			return await _repo.GetCount(x => x.Status == "Preparing" && x.RestaurantId == restaurantId);
		}
		public async Task<long> GetAllOrdersConfirmedStausCountAsync(long restaurantId)
		{
			return await _repo.GetCount(x => x.Status == "Confirmed" && x.RestaurantId == restaurantId);
		}
		public async Task<long> GetAllOrdersOnTheWayStausCountAsync(long restaurantId)
		{
			return await _repo.GetCount(x => x.Status == "OnTheWay" && x.RestaurantId == restaurantId);
		}
		public async Task<long> GetAllOrdersDeliveredStausCountAsync(long restaurantId)
		{
			return await _repo.GetCount(x => x.Status == "Delivered" && x.RestaurantId == restaurantId);
		}
		public async Task<long> GetAllOrdersPendingStausCountAsync(long restaurantId)
		{
			return await _repo.GetCount(x => x.Status == "Pending" && x.RestaurantId == restaurantId);
		}
		public async Task<IEnumerable<Order>> GetAllOrdersAsync()
		{
			return await _repo.GetAllAsync();
		}

		public IEnumerable<OrderShortDetailsDTO> GetAllByFilters(OrderFilter Filter)
		{
			return _repo.GetAllByFilters(Filter);
		}

		public async Task<IEnumerable<Order>> GetOrderByIdAsync(long Id)
		{
			return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "RestaurantTable, OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item, OrderDetails.OrderDetailOptionValues,Customer, DeliveryStaff,Restaurant,Restaurant.RestaurantServiceStaffs,Restaurant.RestaurantCashierStaffs,RestaurantBranch,RestaurantRatings, RestaurantTable, CardScheme, Aggregator, RestaurantWaiter, RestaurantCashierStaff");
		}

		public async Task<IEnumerable<Order>> GetOrderByUserAsync(long CustomerId)
		{
			return await _repo.GetByIdAsync(x => x.CustomerId == CustomerId, ChildObjects: "OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item, RestaurantBranch, RestaurantBranch.Restaurant, RestaurantRatings");
		}

		public async Task<List<Order>> GetOrderByCashierStaffID(long cashierStaffId, DateTime openingDate, DateTime closingDate)
		{
			var result = await _repo.GetByIdAsync(x => x.RestaurantCashierStaffId == cashierStaffId && x.CreationDate >= openingDate && x.CreationDate <= closingDate,
				ChildObjects: "OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item, OrderDetails.MenuItems.Category, Aggregator");

			return result.ToList();
		}

		public async Task<Order> UpdateOrderAsync(Order Entity)
		{
			return await _repo.UpdateAsync(Entity);
		}

		public async Task<IEnumerable<Order>> GetOnGoingOrdersByUserAsync(long CustomerId, long RestaurantId)
		{
			if (RestaurantId == 0)
				return await _repo.GetByIdAsync(x => x.CustomerId == CustomerId &&
								(x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled) && x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered)),
								ChildObjects: "OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item, RestaurantBranch, RestaurantBranch.Restaurant, RestaurantRatings");
			else
				return await _repo.GetByIdAsync(x => x.CustomerId == CustomerId && x.RestaurantId == RestaurantId &&
									(x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled) && x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered)),
									ChildObjects: "OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item, RestaurantBranch, RestaurantBranch.Restaurant, RestaurantRatings");
		}

		public async Task<IEnumerable<Order>> GetPastOrdersByUserAsync(long CustomerId, long RestaurantId)
		{
			if (RestaurantId == 0)
				return await _repo.GetByIdAsync(x => x.CustomerId == CustomerId &&
								(x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled) || x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered)),
								ChildObjects: "OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item, RestaurantBranch, RestaurantBranch.Restaurant, RestaurantRatings");
			else
				return await _repo.GetByIdAsync(x => x.CustomerId == CustomerId && x.RestaurantId == RestaurantId &&
								(x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled) || x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered)),
								ChildObjects: "OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item, RestaurantBranch, RestaurantBranch.Restaurant, RestaurantRatings");
		}

		public async Task<IEnumerable<Order>> GetAllOrdersByRiderAsync(HelperClasses.DTOs.RestaurantDeliveryStaff.RestaurantDeliveryStaffOrderFilterDTO Model)
		{
			if (Model.RequireNewOrders)
				return await _repo.GetByIdAsync(x => x.DeliveryStaffId == Model.DeliveryStaffId &&
								   (x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady) || x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.OnTheWay)),
								   ChildObjects: "OrderDetails", Pagination: Model.Paging, OrderExp: x => x.Id, IsOrderByDescending: true);

			return await _repo.GetByIdAsync(x => x.DeliveryStaffId == Model.DeliveryStaffId &&
								(x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered) || x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered)),
								ChildObjects: "OrderDetails", Pagination: Model.Paging, OrderExp: x => x.Id, IsOrderByDescending: true);
		}

		public async Task<List<OrderDetailOptionsDTO>> GetOptionsByOrderDetail(long OrderDetailId)
		{
			return await _repo.GetOrderOptionsByDetail(OrderDetailId);
		}

		public async Task<IEnumerable<Order>> GetOrderByOrderNoAsync(string OrderNo)
		{
			return await _repo.GetByIdAsync(x => x.OrderNo == OrderNo, ChildObjects: "RestaurantTable, OrderDetails, OrderDetails.MenuItems, OrderDetails.MenuItems.Item, OrderDetails.OrderDetailOptionValues,Customer, DeliveryStaff,Restaurant,Restaurant.RestaurantServiceStaffs,Restaurant.RestaurantCashierStaffs,RestaurantBranch,RestaurantRatings, RestaurantTable, CardScheme, Aggregator, RestaurantWaiter, RestaurantCashierStaff");
		}
	}
}
