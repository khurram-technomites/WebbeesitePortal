using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Order.Filter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class OrderRepo : Repository<Order>, IOrderRepo
    {
        private new readonly FougitoContext _context;
        private readonly IMapper _mapper;

        public OrderRepo(FougitoContext context, ILoggerManager _logger, IMapper mapper) : base(context, _logger)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Order>> GetOrderByCustomerID(long RestaurantId)
        {
            var result = await _context.Orders.Where(x => x.RestaurantId == RestaurantId).GroupBy(p => p.CustomerId).Select(g => g.First()).ToListAsync();
            return result;
        }

        //public async Task<List<Order>> GetOrderByCashierStaffID(long cashierStaffId, DateTime openingDate, DateTime closingDate)
        //{
        //	var result = await _context.Orders
        //		.Include(x => x.OrderDetails).ThenInclude(x => x.MenuItems).ThenInclude(x => x.Item)
        //		.Include(x => x.OrderDetails).ThenInclude(x => x.MenuItems).ThenInclude(x => x.Category)
        //		.Include(x => x.Aggregator)
        //		//.Include(x => x.OrderDetails).ThenInclude(x => x.Category)
        //		.Where(x => x.RestaurantCashierStaffId == cashierStaffId && x.CreationDate >= openingDate && x.CreationDate <= closingDate).ToListAsync();

        //	return result;
        //}

        public IEnumerable<OrderShortDetailsDTO> GetAllByFilters(OrderFilter Filter)
        {
            if (Filter.OnlyNewOrders)
            {
                var result = _context.Orders
                                    .Where(x => x.RestaurantBranchId == Filter.BranchID && x.Origin == Enum.GetName(typeof(OrderOrigin), OrderOrigin.Customer)
                                            //&&
                                            //(x.PaymentMethod != Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Card) || x.IsPaid)
                                            && (x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered) && x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled) &&
                                            x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered))
                                            && (string.IsNullOrEmpty(Filter.Paging.Search) || x.OrderNo.Contains(Filter.Paging.Search))
                                            )
                                    .Select(x => new OrderShortDetailsDTO
                                    {
                                        Id = x.Id,
                                        OrderNo = x.OrderNo,
                                        TotalAmount = x.TotalAmount,
                                        PaymentMethod = x.Address,
                                        Status = x.Status,
                                        IsPaid = x.IsPaid,
                                        Currency = x.Currency,
                                        Date = x.OrderPlacementDateTime,
                                        CustomerName = x.CustomerName
                                    });


                IEnumerable<OrderShortDetailsDTO> Result = Filter.SortBy switch
                {
                    1 => result.OrderByDescending(x => x.Id).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    2 => result.OrderBy(x => x.Id).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    _ => result.Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                };
                return Result;
            }
            else
            {
                var result = _context.Orders
                                    .Where(x => x.RestaurantBranchId == Filter.BranchID && x.Origin == Enum.GetName(typeof(OrderOrigin), OrderOrigin.Customer)
                                            && (x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered) || x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled) ||
                                            x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered))
                                            && (string.IsNullOrEmpty(Filter.Paging.Search) || x.OrderNo.Contains(Filter.Paging.Search))
                                            )
                                    .Select(x => new OrderShortDetailsDTO
                                    {
                                        Id = x.Id,
                                        OrderNo = x.OrderNo,
                                        TotalAmount = x.TotalAmount,
                                        PaymentMethod = x.Address,
                                        Status = x.Status,
                                        IsPaid = x.IsPaid,
                                        Currency = x.Currency,
                                        Date = x.OrderPlacementDateTime,
                                        CustomerName = x.CustomerName
                                    });


                IEnumerable<OrderShortDetailsDTO> Result = Filter.SortBy switch
                {
                    1 => result.OrderByDescending(x => x.Id).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    2 => result.OrderBy(x => x.Id).Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                    _ => result.Skip((Filter.Paging.PageNumber - 1) * Filter.Paging.PageSize).Take(Filter.Paging.PageSize).ToList(),
                };
                return Result;
            }
        }

        public async Task<List<OrderDetailOptionsDTO>> GetOrderOptionsByDetail(long OrderDetailId)
        {
            var result = await _context.OrderDetailOptionValues.Where(x => x.OrderDetailId == OrderDetailId).ToListAsync();

            List<OrderDetailOptionsDTO> filter = (from MT in result
                                                  group MT by new
                                                  {
                                                      MT.MenuItemOption,
                                                      MT.OrderDetail.CustomerNote
                                                  } into g
                                                  select new OrderDetailOptionsDTO
                                                  {
                                                      CustomerNote = g.Key.CustomerNote,
                                                      Option = g.Key.MenuItemOption,
                                                      OptionValues = _mapper.Map<List<OrderDetailsOptionValuesDTO>>(g.ToList())
                                                  }).ToList();

            return filter;
        }

       
    }
}
