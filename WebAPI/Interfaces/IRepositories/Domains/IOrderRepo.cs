using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Order.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
	public interface IOrderRepo : IRepository<Order>
	{
		IEnumerable<OrderShortDetailsDTO> GetAllByFilters(OrderFilter Filter);
		Task<IEnumerable<Order>> GetOrderByCustomerID(long RestaurantId);
		//Task<List<Order>> GetOrderByCashierStaffID(long cashierStaffId, DateTime openingDate, DateTime closingDate);
		Task<List<OrderDetailOptionsDTO>> GetOrderOptionsByDetail(long OrderDetailId);
	}
}
