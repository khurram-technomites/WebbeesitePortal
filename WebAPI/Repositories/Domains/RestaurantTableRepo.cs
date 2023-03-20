using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
	public class RestaurantTableRepo : Repository<RestaurantTable>, IRestaurantTableRepo
	{
		private new readonly FougitoContext _context;
		private readonly IMapper _mapper;

		public RestaurantTableRepo(FougitoContext context, ILoggerManager _logger, IMapper mapper) : base(context, _logger)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<RestaurantTableDTO>> GetReservedByRestaurantBranchID(long RestaurantBranchId, string Status)
		{
			List<RestaurantTableDTO> result = (from T in _context.RestaurantTables
						  join O in _context.Orders on T.Id equals O.RestaurantTableId
						  where T.Status == Status && T.RestaurantBranchId == RestaurantBranchId
								&&
								(O.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Pending)
								|| O.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed)
								|| O.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Preparing)
								|| O.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady)
								|| O.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.OnTheWay)
								|| O.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered)
								|| O.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered)
								)
								&& O.Origin == Enum.GetName(typeof(OrderOrigin), OrderOrigin.POS)
							orderby T.Id

						  select new RestaurantTableDTO
						  {
							  Id = T.Id,
							  Name = T.Name,
							  Type = T.Type,
							  Status = T.Status,
							  Serving = T.Serving,
							  CreationDate = T.CreationDate,
							  RestaurantId = T.RestaurantId,
							  RestaurantBranchId = T.RestaurantBranchId,
							  Restaurant = null,
							  RestaurantBranch = null,
							  RestaurantTableReservations = null,
							  OrderId = O.Id,
							  OrderNo = O.OrderNo,
						  }).ToList();

			return result;
		}
	}
}
