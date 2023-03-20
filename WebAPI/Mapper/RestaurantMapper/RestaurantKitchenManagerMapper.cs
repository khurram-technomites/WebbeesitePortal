using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantKitchenManager;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
	public class RestaurantKitchenManagerMapper : Profile
	{
		public RestaurantKitchenManagerMapper()
		{
			CreateMap<RestaurantKitchenManager, RestaurantKitchenManagerDTO>()
			   .ForMember(des => des.RestaurantBranchName, opt => opt.MapFrom(src => src.RestaurantBranch != null ? src.RestaurantBranch.NameAsPerTradeLicense : "-"));
			CreateMap<RestaurantKitchenManagerDTO, RestaurantKitchenManager>();
		}
	}
}
