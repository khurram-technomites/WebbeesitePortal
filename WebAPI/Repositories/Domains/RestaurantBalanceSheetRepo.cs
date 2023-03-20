using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Restaurant;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;
using AutoMapper;
using System.Collections.Generic;
using WebAPI.Helpers;
using System;
using HelperClasses.Classes;

namespace WebAPI.Repositories.Domains
{
	public class RestaurantBalanceSheetRepo : Repository<RestaurantBalanceSheet>, IRestaurantBalanceSheetRepo
	{
		private new readonly FougitoContext _context;
		private readonly IMapper _mapper;
		private int i = 1;

		public RestaurantBalanceSheetRepo(FougitoContext context, ILoggerManager logger, IMapper mapper) : base(context, logger)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<RestaurantBalanceSheetReportDTO> GetReportyRestaurantCashierStaffIdAsync(long CashierStaffId, long? Id)
		{
			RestaurantBalanceSheet currentShift = new();
			RestaurantBalanceSheet sheet = new();
			RestaurantBalanceSheetReportDTO report = new();
			List<Order> totalOrders, completedOrders, canceledOrders = new();

			//Old Shift
			if (Id != null && Id > 0)
			{
				sheet = await _context.RestaurantBalanceSheets.Where(x => x.RestaurantCashierStaffId == CashierStaffId && x.Id == Id).FirstOrDefaultAsync();
			}
			else
			{
				sheet = await _context.RestaurantBalanceSheets.Where(x => x.RestaurantCashierStaffId == CashierStaffId).FirstOrDefaultAsync();
			}

			if (sheet == null)
			{
				return null;
			}

			DateTime openingDate = sheet.OpeningDate.Value;
			DateTime closingDate = sheet.ClosingDate ?? DateTime.UtcNow.ToDubaiDateTime();
			//DateTime closingDate = DateTime.UtcNow.ToDubaiDateTime();

			if (sheet.Status == Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened))
			{
				List<OrderDetail> orderDetails = new();

				currentShift = await _context.RestaurantBalanceSheets.Where(x => x.Id == Id)
				.Include(x => x.RestaurantBranch)
				.Include(x => x.restaurantCashierStaff).ThenInclude(x => x.Orders.Where(o => o.CreationDate >= openingDate && o.CreationDate <= closingDate)).ThenInclude(x => x.OrderDetails)
					.ThenInclude(x => x.MenuItems).ThenInclude(x => x.Category)
				.Include(x => x.restaurantCashierStaff).ThenInclude(x => x.Orders.Where(o => o.CreationDate >= openingDate && o.CreationDate <= closingDate)).ThenInclude(x => x.Aggregator)
				.Include(x => x.RestaurantCashDenomination).ThenInclude(x => x.RestaurantCashDenominationDetails).ThenInclude(x => x.CurrencyNote)
				//.Include(x => x.RestaurantAggregatorWiseSales).ThenInclude(x => x.Order).ThenInclude(x => x.Aggregator)
				//.Include(x => x.RestaurantProductWiseSales).ThenInclude(x => x.OrderDetail).ThenInclude(x => x.MenuItems).ThenInclude(x => x.Category)
				//.Include(x => x.RestaurantCategoryWiseSales).ThenInclude(x => x.OrderDetail).ThenInclude(x => x.MenuItems).ThenInclude(x => x.Category)
				.FirstOrDefaultAsync();

				report = _mapper.Map<RestaurantBalanceSheetReportDTO>(currentShift);

				totalOrders = currentShift.restaurantCashierStaff.Orders.ToList();
				completedOrders = currentShift.restaurantCashierStaff.Orders.Where(x => x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled)).ToList();
				canceledOrders = currentShift.restaurantCashierStaff.Orders.Where(x => x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled)).ToList();

				report.TotalOrderCount = totalOrders.Count;
				report.CompletedOrderCount = totalOrders.Count;
				report.CanceledOrderCount = canceledOrders.Count;

				if (totalOrders.Count < 1)
					return report;

				orderDetails = completedOrders.SelectMany(x => x.OrderDetails).ToList();

				/*Aggregator Wise*/
				List<Order> aggregatorOrders = completedOrders.Where(obj => obj.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Aggregator)).ToList();

				i = 1;
				foreach (var order in aggregatorOrders)
				{
					report.RestaurantAggregatorWiseSales.Add(new RestaurantAggregatorWiseSaleDTO()
					{
						Id = Increment(ref i),
						RestaurantBalanceSheetId = sheet.Id,
						OrderId = order.Id,
						RestaurantAggregatorId = order.Aggregator.Id,
						RestaurantAggregatorName = order.Aggregator.Name,
						PaymentStatus = order.PaidTo,
						Amount = order.TotalAmount,
					});
				};
				//group by

				foreach (var order in completedOrders)
				{
					foreach (var orderDetail in order.OrderDetails)
					{
						/*Product Wise*/
						report.RestaurantProductWiseSales.Add(new RestaurantProductWiseSaleDTO()
						{
							Id = Increment(ref i),
							RestaurantBalanceSheetId = sheet.Id,
							Quantity = orderDetail.Quantity,
							OrderDetailId = orderDetail.Id,
							MenuItemId = orderDetail.MenuItems.Id,
							MenuItemName = orderDetail.MenuItems.Name,
							Amount = orderDetail.TotalPrice, //It may change after testing ... asim
						});

						/*Category Wise*/
						report.RestaurantCategoryWiseSales.Add(new RestaurantCategoryWiseSaleDTO()
						{
							Id = Increment(ref i),
							RestaurantBalanceSheetId = sheet.Id,
							Quantity = orderDetail.Quantity,
							OrderDetailId = orderDetail.Id,
							CategoryId = orderDetail.MenuItems.Category.Id,
							CategoryName = orderDetail.MenuItems.Category.Name,
							Amount = orderDetail.TotalPrice, //It may change after testing ... asim
						});
					}
				};

				/*Balance Sheet Total*/
				//report.Id = currentShift.Id;
				//report.DeviceId = currentShift.DeviceId;
				//report.OpeningBalance = currentShift.OpeningBalance;
				//report.ClosingBalance = currentShift.ClosingBalance;
				//report.Status = currentShift.Status;
				//report.RestaurantId = currentShift.RestaurantId;
				//report.RestaurantBranchId = currentShift.RestaurantBranchId;
				//report.RestaurantCashierStaffId = currentShift.RestaurantCashierStaffId;
				//report.RestaurantCashDenomination = _mapper.Map<RestaurantCashDenominationDTO>(currentShift.RestaurantCashDenomination);

				report.AggregatorOnlineSale = report.RestaurantAggregatorWiseSales.Where(x => x.PaymentStatus == Enum.GetName(typeof(PaymentStatus), PaymentStatus.PaidOnline)).Sum(x => x.Amount);
				report.AggregatorCashSale = report.RestaurantAggregatorWiseSales.Where(x => x.PaymentStatus == Enum.GetName(typeof(PaymentStatus), PaymentStatus.PaidToRestaurant)).Sum(x => x.Amount);

				report.TotalCashSale = completedOrders.Sum(x => (x.CashReceived - x.Change));

				report.TotalCardSale = completedOrders.Sum(x => x.CardAmount);

				report.TotalCreditSale = completedOrders.Where(x => x.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Credit)).Sum(x => x.TotalAmount);

				report.GrandTotal = completedOrders.Sum(x => x.TotalAmount);
				report.TotalTax = completedOrders.Sum(x => x.TaxAmount);

				report.NetTotal = report.GrandTotal - report.TotalTax;
				report.TotalItemQuantity = report.RestaurantProductWiseSales.Sum(x => x.Quantity);
				report.TotalCategoryCount = report.RestaurantCategoryWiseSales.Sum(x => x.Id);
				report.DeliveryCharges = completedOrders.Sum(x => x.DeliveryCharges);
				report.TotalDiscount = completedOrders.Sum(x => x.DiscountAmount) + orderDetails.Sum(x => x.DiscountAmount);

				report.TotalSaleWithoutTax = report.GrandTotal - report.TotalTax;
				report.GrossSaleWithTax = report.GrandTotal;
			}
			else
			{
				currentShift = await _context.RestaurantBalanceSheets.Where(x => x.Id == Id)
				//.Include(x => x.RestaurantBranch)
				.Include(x => x.restaurantCashierStaff).ThenInclude(x => x.Orders.Where(o => o.CreationDate >= openingDate && o.CreationDate <= closingDate))
				//	.ThenInclude(x => x.OrderDetails)
				//	.ThenInclude(x => x.MenuItems).ThenInclude(x => x.Category)
				//.Include(x => x.restaurantCashierStaff).ThenInclude(x => x.Orders.Where(o => o.CreationDate >= openingDate && o.CreationDate <= closingDate)).ThenInclude(x => x.Aggregator)
				.Include(x => x.RestaurantCashDenomination).ThenInclude(x => x.RestaurantCashDenominationDetails).ThenInclude(x => x.CurrencyNote)
				.Include(x => x.RestaurantAggregatorWiseSales).ThenInclude(x => x.RestaurantAggregator)
				.Include(x => x.RestaurantProductWiseSales).ThenInclude(x => x.MenuItem)
				.Include(x => x.RestaurantCategoryWiseSales).ThenInclude(x => x.Category)
				.FirstOrDefaultAsync();

				totalOrders = currentShift.restaurantCashierStaff.Orders.ToList();
				completedOrders = currentShift.restaurantCashierStaff.Orders.Where(x => x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled)).ToList();
				canceledOrders = currentShift.restaurantCashierStaff.Orders.Where(x => x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled)).ToList();

				report = _mapper.Map<RestaurantBalanceSheetReportDTO>(currentShift);

				report.TotalOrderCount = totalOrders.Count;
				report.CompletedOrderCount = totalOrders.Count;
				report.CanceledOrderCount = canceledOrders.Count;

				report.NetTotal = report.GrandTotal - report.TotalTax;
				report.TotalItemQuantity = report.RestaurantProductWiseSales.Sum(x => x.Quantity);
				report.TotalCategoryCount = report.RestaurantCategoryWiseSales.Sum(x => x.Id);
				report.TotalSaleWithoutTax = report.GrandTotal - report.TotalTax;
				report.GrossSaleWithTax = report.GrandTotal;
			}

			//Group by for report
			if (report.RestaurantAggregatorWiseSales.Count > 0)
			{
				var aggregatorList = report.RestaurantAggregatorWiseSales;
				report.RestaurantAggregatorWiseSales = new List<RestaurantAggregatorWiseSaleDTO>();

				i = 1;
				aggregatorList.GroupBy(g => new { g.RestaurantAggregatorName, g.PaymentStatus }).ToList()
				.ForEach(g => report.RestaurantAggregatorWiseSales.Add(g.OrderByDescending(s => s.Amount).Select(a =>
				new RestaurantAggregatorWiseSaleDTO
				{
					Id = Increment(ref i),
					RestaurantAggregatorName = !string.IsNullOrEmpty(a.RestaurantAggregatorName) ? a.RestaurantAggregatorName : a.RestaurantAggregator.Name,
					//RestaurantAggregatorName = a.RestaurantAggregatorName,
					PaymentStatus = a.PaymentStatus,
					Amount = a.Amount,
					RestaurantBalanceSheetId = a.RestaurantBalanceSheetId,
					OrderId = a.OrderId,
					RestaurantAggregatorId = a.RestaurantAggregatorId,
				})
				.First()));
			}

			if (report.RestaurantProductWiseSales.Count > 0)
			{
				var productList = report.RestaurantProductWiseSales;
				report.RestaurantProductWiseSales = new List<RestaurantProductWiseSaleDTO>();

				i = 1;
				// Insert some objects into oldprodList object.
				productList.GroupBy(g => g.MenuItemName).ToList()
					.ForEach(g => report.RestaurantProductWiseSales.Add(g.OrderByDescending(s => s.Quantity).Select(a =>
					new RestaurantProductWiseSaleDTO
					{
						Id = Increment(ref i),
						MenuItemName = !string.IsNullOrEmpty(a.MenuItemName) ? a.MenuItemName : a.MenuItem.Name,
						//MenuItemName = a.MenuItemName,
						Quantity = g.Sum(x => x.Quantity),
						Amount = g.Sum(x => x.Amount),
						RestaurantBalanceSheetId = a.RestaurantBalanceSheetId,
						OrderDetailId = a.OrderDetailId,
						MenuItemId = a.MenuItemId,
					})
					.First()));
			}

			if (report.RestaurantCategoryWiseSales.Count > 0)
			{
				var categoryList = report.RestaurantCategoryWiseSales;
				report.RestaurantCategoryWiseSales = new List<RestaurantCategoryWiseSaleDTO>();

				i = 1;
				// Insert some objects into oldcatList object.
				categoryList.GroupBy(g => g.CategoryName).ToList()
					.ForEach(g => report.RestaurantCategoryWiseSales.Add(g.OrderByDescending(s => s.Quantity).Select(a =>
					new RestaurantCategoryWiseSaleDTO
					{
						Id = Increment(ref i),
						CategoryName = !string.IsNullOrEmpty(a.CategoryName) ? a.CategoryName : a.OrderDetail.Category.Name,
						//CategoryName = a.CategoryName,
						Quantity = g.Sum(x => x.Quantity),
						Amount = g.Sum(x => x.Amount),
						RestaurantBalanceSheetId = a.RestaurantBalanceSheetId,
						OrderDetailId = a.OrderDetailId,
						CategoryId = a.CategoryId,
					})
					.First()));
			}

			return report;
		}

		//public async Task<RestaurantBalanceSheet> GetShiftDetails(RestaurantBalanceSheet balanceSheet)
		//{
		//	balanceSheet.restaurantCashierStaff.Orders = await _context.Orders
		//		.Include(x => x.OrderDetails).ThenInclude(x => x.MenuItems).ThenInclude(x => x.Item)
		//		.Include(x => x.OrderDetails).ThenInclude(x => x.MenuItems).ThenInclude(x => x.Category)
		//		.Include(x => x.Aggregator)
		//		//.Include(x => x.OrderDetails).ThenInclude(x => x.Category)
		//		.Where(x => x.RestaurantCashierStaffId == balanceSheet.RestaurantCashierStaffId && x.CreationDate >= balanceSheet.OpeningDate && x.CreationDate <= balanceSheet.ClosingDate).ToListAsync();

		//	return balanceSheet;
		//}

		private static int Increment(ref int i)
		{
			return i++;
		}
	}
}
