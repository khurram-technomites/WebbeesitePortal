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
	public class RestaurantReservationRepo : Repository<RestaurantReservation>, IRestaurantReservationRepo
	{
		private new readonly FougitoContext _context;
		private readonly IMapper _mapper;

		public RestaurantReservationRepo(FougitoContext context, ILoggerManager _logger, IMapper mapper) : base(context, _logger)
		{
			_context = context;
			_mapper = mapper;
		}

	}
}
