using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
	public class RestaurantWaiterMapper : Profile
	{
		public RestaurantWaiterMapper()
		{
			CreateMap<RestaurantWaiter, RestaurantWaiterDTO>()
				.ForMember(des => des.RestaurantBranchName, opt => opt.MapFrom(src => src.RestaurantBranch != null ? src.RestaurantBranch.NameAsPerTradeLicense : "-"))
				.ForMember(des => des.Logo, opt => opt.MapFrom(src => src.Logo != null ? src.Logo : "https://cdn.fougitodemo.com/images/RestaurantWaiterStaff/waiter.jpg"))
			;
			CreateMap<RestaurantWaiterDTO, RestaurantWaiter>()
				.ForMember(des => des.Logo, opt => opt.MapFrom(src => src.Logo != null ? src.Logo : "https://cdn.fougitodemo.com/images/RestaurantWaiterStaff/waiter.jpg"));
		}
	}
}
