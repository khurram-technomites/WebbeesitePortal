using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
	public class RestaurantManagerMapper : Profile
	{
		public RestaurantManagerMapper()
		{
			CreateMap<RestaurantManager, RestaurantManagerDTO>()
				.ForMember(des => des.RestaurantBranchName, opt => opt.MapFrom(src => src.RestaurantBranch != null ? src.RestaurantBranch.NameAsPerTradeLicense : "-"));
			CreateMap<RestaurantManagerDTO, RestaurantManager>();

		}
	}
}
