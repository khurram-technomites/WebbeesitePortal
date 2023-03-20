using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantDeliveryStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Mapper.OrderMapper
{
	public class OrderMapper : Profile
	{
		public OrderMapper()
		{
			CreateMap<Order, OrderDTO>()
				.ForMember(x => x.EstimatedDeliveryMinutes, x => x.MapFrom(y => (y.DeliveryDateTime.Value.AddMinutes(y.EstimatedDeliveryMinutes) - DateTime.UtcNow.ToDubaiDateTime()).Minutes))
				.ForMember(x => x.PaidStatus, x => x.MapFrom(y => y.IsPaid == true ? "Paid" : "UnPaid"))
				;
			CreateMap<OrderDTO, Order>()
				.ForAllOtherMembers(o => o.Condition((source, destination, member) => member != null));

			CreateMap<OrderPlacementDTO, Order>()
				//.ForMember(x => x.OrderDetails, x => x.MapFrom(y => y.OrderItems))
				.ForMember(x => x.Latitude, x => x.MapFrom(src => (src.Latitude != null) ? src.Latitude : 0))
				.ForMember(x => x.Longitude, x => x.MapFrom(src => (src.Longitude != null) ? src.Longitude : 0))
				.ForMember(des => des.OrderDetails, des => des.MapFrom(src => (src.OrderId == 0 || src.OrderId == null) ? src.OrderItems : null))
				.ForMember(x => x.DiscountPercent, x => x.MapFrom(y => y.DiscountPercentage))
				.ForMember(x => x.Amount, x => x.MapFrom(y => y.Subtotal))
				.ForMember(x => x.TaxPercent, x => x.MapFrom(y => y.TaxPercentage))
				.ForMember(x => x.RestaurantTableId, x => x.MapFrom(y => y.TableId))
				.ForMember(x => x.DeliveryType, x => x.MapFrom(y => y.DeliveryType))
				.AfterMap<SetRemainingColumns>()
				.ForAllOtherMembers(o => o.Condition((source, destination, member) => member != null))
				;

			CreateMap<OrderDetailDTO, OrderDetail>();

			CreateMap<List<Order>, DashboardStatsDTO>()
				.ForMember(x => x.Cash, x => x.MapFrom(y => y.Where(x => x.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Cash)).Sum(z => z.Amount)))
				.ForMember(x => x.Card, x => x.MapFrom(y => y.Where(x => x.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Card)).Sum(z => z.Amount)))
				.ForMember(x => x.Partial, x => x.MapFrom(y => y.Where(x => x.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Partial)).Sum(z => z.Amount)))
				.ForMember(x => x.Credit, x => x.MapFrom(y => y.Where(x => x.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Credit)).Sum(z => z.Amount)))
				.ForMember(x => x.Aggregator, x => x.MapFrom(y => y.Where(x => x.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Aggregator)).Sum(z => z.Amount)));

			CreateMap<Order, RestaurantDeliveryStaffCashDTO>()
				.ForMember(des => des.DeliveryStaffName, des => des.MapFrom(src => (src.DeliveryStaff != null ? src.DeliveryStaff.FirstName + " " + src.DeliveryStaff.LastName : "-")));

			CreateMap<OrderDTO, RestaurantPendingPaymentsDTO>();

		}

		private class SetRemainingColumns : IMappingAction<OrderPlacementDTO, Order>
		{
			private readonly IMenuItemService _menuItemService;
			private readonly IMenuItemOptionService _menuItemOptionService;
			private readonly IOrderService _orderService;
			public SetRemainingColumns(IMenuItemService menuItemService, IMenuItemOptionService menuItemOptionService, IOrderService orderService)
			{
				_menuItemService = menuItemService;
				_menuItemOptionService = menuItemOptionService;
				_orderService = orderService;
			}

			public void Process(OrderPlacementDTO source, Order destination, ResolutionContext context)
			{
				//mapper config
				MapperConfiguration detailMapper = new(cfg =>
				{
					cfg.CreateMap<OrderItem, OrderDetail>()
						.ForMember(x => x.OrderDetailOptionValues, x => x.MapFrom(y => y.OrderItemOptions));
					cfg.CreateMap<OrderDetail, OrderItem>()
						.ForMember(x => x.OrderItemOptions, x => x.MapFrom(y => y.OrderDetailOptionValues));

					cfg.CreateMap<OrderItemOption, OrderDetailOptionValue>();
					cfg.CreateMap<OrderDetailOptionValue, OrderItemOption>();

					cfg.CreateMap<OrderDetailOptionValue, OrderDetailsOptionValuesDTO>();
					cfg.CreateMap<OrderDetailsOptionValuesDTO, OrderDetailOptionValue>();
				});

				destination.Currency = "AED";

				if (!destination.OrderDetails.Any())
				{
					var OldOrder = _orderService.GetOrderByIdAsync(destination.Id).Result;
					destination.OrderDetails = OldOrder.FirstOrDefault().OrderDetails; //old details
				}

				if (source.OrderId != null && source.OrderId > 0)
				{
					var srcItems = source.OrderItems;// new details
					var desItems = destination.OrderDetails.ToList();// old details

					foreach (var item in srcItems)
					{
						OrderDetail desItem = desItems.Find(x => x.Id == item.Id);
						// New Item
						if (desItem == null)
						{
							desItem = detailMapper.CreateMapper().Map<OrderDetail>(item);
							destination.OrderDetails.Add(desItem);
						}
						// Edit Item
						else if (desItem != null)
						{
							if (item.IsUpdated.HasValue && item.IsUpdated == true)
							{
								desItem = destination.OrderDetails.FirstOrDefault(x => x.Id == desItem.Id);
								if (desItem != null)
								{
									destination.OrderDetails.Remove(desItem);
									desItem = detailMapper.CreateMapper().Map<OrderDetail>(item);
									//desItem.EditCount = desItem.EditCount + 1;
									destination.OrderDetails.Add(desItem);
								}
							}
						}
					}
				}

				foreach (var OrderDetail in destination.OrderDetails)
				{
					OrderDetail.MenuItems = null;

					if (source.OrderId != null && source.OrderId > 0) //Order Update Case
					{
						OrderItem detailItem = source.OrderItems.Find(x => x.Id == OrderDetail.Id);

						if (detailItem == null)
						{
							OrderDetail.Status = Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled);
							OrderDetail.OrderDetailOptionValues = new List<OrderDetailOptionValue>();
						}
						else if (detailItem.IsUpdated.HasValue && detailItem.IsUpdated == true)
						{
							OrderDetail.Status = Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Updated);
							OrderDetail.IsUpdated = true;
							OrderDetail.EditCount = OrderDetail.EditCount + 1;
							OrderDetail.OrderDetailOptionValues = new List<OrderDetailOptionValue>();
						}
						else if (detailItem.Id > 1)
						{
							OrderDetail.OrderDetailOptionValues = new List<OrderDetailOptionValue>();
						}
						else
						{
							//For new item in kitchen slip
							if (string.IsNullOrEmpty(OrderDetail.Status))
							{
								OrderDetail.Status = Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.New);
							}
						}
					}

					IEnumerable<MenuItem> MenuItems = _menuItemService.GetById(OrderDetail.MenuItemId).Result;
					MenuItem MenuItem = MenuItems.FirstOrDefault();

					OrderDetail.MenuItemName = MenuItem.Name;

					//ignoring here to map from selected option => line 70
					if (MenuItem.Price != 0)
					{
						OrderDetail.Price = MenuItem.Price;
						OrderDetail.UnitPrice = OrderDetail.Quantity * MenuItem.Price;
					}

					//OrderDetail.TaxAmount = 0;
					//OrderDetail.TaxPercent = 0;
					//OrderDetail.DiscountAmount = 0;
					//OrderDetail.DiscountPercent = 0;

					foreach (var Option in OrderDetail.OrderDetailOptionValues)
					{
						IEnumerable<MenuItemOption> Options = _menuItemOptionService.GetById(Option.MenuItemOptionId.Value).Result;
						MenuItemOption OptionFirst = Options.FirstOrDefault();

						var SelectedOption = OptionFirst.MenuItemOptionValues.Where(x => x.Id == Option.MenuItemOptionValueId).FirstOrDefault();
						Option.Quantity = 1;
						//if (!OptionFirst.IsPriceMain) //Avoiding map if option price is also main price. This is important
						//{
						Option.UnitPrice = SelectedOption.Price;
						Option.Price = Option.Quantity * SelectedOption.Price;
						Option.TotalPrice = Option.Price * Option.Quantity;
						//}
						//else //Mapping main prices in OrderDetails
						//{                            
						//    OrderDetail.UnitPrice = SelectedOption.Price;
						//    OrderDetail.Price = OrderDetail.Quantity * SelectedOption.Price;
						//}

						Option.MenuItemOption = OptionFirst.Title;
						Option.MenuItemOptionValue = SelectedOption.Value;
					}
				}
			}
		}
	}
}
