using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Linq;
using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
	public class RestaurantBalanceSheetMapper : Profile
	{
		private int i = 1;

		public RestaurantBalanceSheetMapper()
		{
			/* Basic DTO Mapping */
			CreateMap<RestaurantBalanceSheet, RestaurantBalanceSheetDTO>()
				.ForMember(x => x.Orders, x => x.MapFrom(y => y.restaurantCashierStaff.Orders))
					.AfterMap((src, dest) =>
					{
						if (src.restaurantCashierStaff == null && src.restaurantCashierStaff.Orders.Count == 0)
						{
							return;
						}
						else if (src.restaurantCashierStaff.Orders.Where(x => x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled)).ToList().Count == 0)
						{
							//if there is no completed order
							return;
						}
						List<Order> orders = src.restaurantCashierStaff.Orders.Where(x => x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled)).ToList();
						List<OrderDetail> orderDetails = orders.SelectMany(x => x.OrderDetails).ToList();

						/*Aggregator Wise*/
						List<Order> aggregatorOrders = orders.Where(obj => obj.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Aggregator)).ToList();

						foreach (var order in aggregatorOrders)
						{
							dest.RestaurantAggregatorWiseSales.Add(new RestaurantAggregatorWiseSaleDTO()
							{
								RestaurantBalanceSheetId = src.Id,
								OrderId = order.Id,
								RestaurantAggregatorId =  order.AggregatorId,
								RestaurantAggregatorName = order.Aggregator != null ? order.Aggregator.Name : "",
								PaymentStatus = order.PaidTo,
								Amount = order.TotalAmount, //It may change after testing ... asim
							});
						}

						foreach (var order in orders)
						{
							foreach (var orderDetail in order.OrderDetails)
							{
								/*Product Wise*/
								dest.RestaurantProductWiseSales.Add(new RestaurantProductWiseSaleDTO()
								{
									RestaurantBalanceSheetId = src.Id,
									Quantity = orderDetail.Quantity,
									OrderDetailId = orderDetail.Id,
									MenuItemId = orderDetail.MenuItems != null ? orderDetail.MenuItems.Id : null,
									MenuItemName = orderDetail.MenuItems != null ? orderDetail.MenuItems.Name: "",
									Amount = orderDetail.TotalPrice, //It may change after testing ... asim
								});

								/*Category Wise*/
								dest.RestaurantCategoryWiseSales.Add(new RestaurantCategoryWiseSaleDTO()
								{
									RestaurantBalanceSheetId = src.Id,
									Quantity = orderDetail.Quantity,
									OrderDetailId = orderDetail.Id,
									CategoryId = orderDetail.MenuItems != null ? (orderDetail.MenuItems.Category != null ? orderDetail.MenuItems.Category.Id : null) : null,
									CategoryName = orderDetail.MenuItems != null ? (orderDetail.MenuItems.Category != null ?orderDetail.MenuItems.Category.Name: "") : "",
									Amount = orderDetail.TotalPrice, //It may change after testing ... asim
								});
							}
						}

						/*Balance Sheet Total*/
						dest.AggregatorOnlineSale = dest.RestaurantAggregatorWiseSales.Where(x => x.PaymentStatus == Enum.GetName(typeof(PaymentStatus), PaymentStatus.PaidOnline)).Sum(x => x.Amount);
						dest.AggregatorCashSale = dest.RestaurantAggregatorWiseSales.Where(x => x.PaymentStatus == Enum.GetName(typeof(PaymentStatus), PaymentStatus.PaidToRestaurant)).Sum(x => x.Amount);

						dest.TotalCashSale = orders.Sum(x => (x.CashReceived - x.Change));

						dest.TotalCardSale = orders.Sum(x => x.CardAmount);

						dest.TotalCreditSale = orders.Where(x => x.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Credit)).Sum(x => x.TotalAmount);

						dest.GrandTotal = orders.Sum(x => x.TotalAmount);
						dest.TotalTax = orders.Sum(x => x.TaxAmount);

						dest.NetTotal = dest.GrandTotal - dest.TotalTax;
						dest.TotalItemQuantity = dest.RestaurantProductWiseSales.Sum(x => x.Quantity);
						dest.TotalCategoryCount = dest.RestaurantCategoryWiseSales.Sum(x => x.Id);
						dest.DeliveryCharges = orders.Sum(x => x.DeliveryCharges);
						dest.TotalDiscount = orders.Sum(x => x.DiscountAmount) + orderDetails.Sum(x => x.DiscountAmount);

						dest.TotalSaleWithoutTax = dest.GrandTotal - dest.TotalTax;
						dest.GrossSaleWithTax = dest.GrandTotal;

						//remove unnecessory things
						if (dest.IsUpdation)
						{
							src.restaurantCashierStaff.Orders = new List<Order>();
							dest.RestaurantAggregatorWiseSales.ForEach(x => x.RestaurantAggregator = null);
							dest.RestaurantProductWiseSales.ForEach(x => x.MenuItem = null);
							dest.RestaurantProductWiseSales.ForEach(x => x.OrderDetail = null);
							dest.RestaurantCategoryWiseSales.ForEach(x => x.Category = null);
							dest.RestaurantCategoryWiseSales.ForEach(x => x.OrderDetail = null);
						}

					})
				.ForAllOtherMembers(x => x.Condition((source, destination, member) => member != null))
			;
			CreateMap<RestaurantBalanceSheetDTO, RestaurantBalanceSheet>()
				.ForAllMembers(x => x.Condition((source, destination, member) => member != null))
			//.ForMember(x => x.RestaurantId, y => y.Condition(x => (x.RestaurantId != 0)))
			//.ForMember(x => x.RestaurantBranchId, y => y.Condition(x => (x.RestaurantBranchId != 0)))
			//.ForMember(x => x.RestaurantCashierStaffId, y => y.Condition(x => (x.RestaurantCashierStaffId != 0)))
			//.ForAllOtherMembers(x => x.Condition((source, destination, member) => member != null))
			;

			/* Calculate Variance */

			//Balance Sheet
			//CreateMap<RestaurantBalanceSheet, RestaurantBalanceSheetVarianceDTO>()
			//	.ForMember(x => x.IsPositiveVariance, x => x.MapFrom(y => y.GrandTotal >= y.ClosingBalance))
			//	.ForMember(x => x.TotalAmount, x => x.MapFrom(y => y.ClosingBalance))
			//	.ForMember(x => x.PositiveVariance, x => x.MapFrom(y => y.GrandTotal >= y.ClosingBalance ? y.GrandTotal - y.ClosingBalance : 0))
			//	.ForMember(x => x.NegativeVariance, x => x.MapFrom(y => y.GrandTotal <= y.ClosingBalance ? y.GrandTotal - y.ClosingBalance : 0))
			//	;
			//CreateMap<RestaurantBalanceSheetVarianceDTO, RestaurantBalanceSheet>();

			//Balance Sheet DTO
			CreateMap<RestaurantBalanceSheetDTO, RestaurantBalanceSheetVarianceDTO>()
				.ForMember(x => x.TotalCashSale, x => x.MapFrom(y => y.TotalCashSale))
				.ForMember(x => x.IsPositiveVariance, x => x.MapFrom(y => y.ClosingBalance >= (y.OpeningBalance + y.TotalCashSale)))
				.ForMember(x => x.TotalAmount, x => x.MapFrom(y => y.ClosingBalance))
				.ForMember(x => x.PositiveVariance, x => x.MapFrom(y => y.ClosingBalance >= (y.OpeningBalance + y.TotalCashSale) ? y.ClosingBalance - (y.OpeningBalance + y.TotalCashSale) : 0))
				.ForMember(x => x.NegativeVariance, x => x.MapFrom(y => y.ClosingBalance < (y.OpeningBalance + y.TotalCashSale) ? ((y.OpeningBalance + y.TotalCashSale) - y.ClosingBalance) : 0))
				;
			CreateMap<RestaurantBalanceSheetVarianceDTO, RestaurantBalanceSheetDTO>();

			/* Report DTO Mapping */

			//Balance Sheet
			CreateMap<RestaurantBalanceSheet, RestaurantBalanceSheetReportDTO>()
				.ForAllOtherMembers(x => x.Condition((source, destination, member) => member != null))
				;
			CreateMap<RestaurantBalanceSheetReportDTO, RestaurantBalanceSheet>()
				.ForMember(x => x.RestaurantId, y => y.Condition(x => (x.RestaurantId != 0)))
				.ForMember(x => x.RestaurantBranchId, y => y.Condition(x => (x.RestaurantBranchId != 0)))
				.ForMember(x => x.RestaurantCashierStaffId, y => y.Condition(x => (x.RestaurantCashierStaffId != 0)))
				.ForAllOtherMembers(x => x.Condition((source, destination, member) => member != null))
				;

			//Logs
			CreateMap<RestaurantBalanceSheetDTO, RestaurantBalanceSheetLogsDTO>();
			CreateMap<RestaurantBalanceSheetLogsDTO, RestaurantBalanceSheetDTO>();

		}

		private static int Increment(ref int i)
		{
			i = i + 1;
			return i;
		}
	}
}
