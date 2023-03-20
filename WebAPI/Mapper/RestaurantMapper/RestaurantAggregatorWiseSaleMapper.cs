using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
	public class RestaurantAggregatorWiseSaleMapper : Profile
	{
		public RestaurantAggregatorWiseSaleMapper()
		{
			CreateMap<RestaurantAggregatorWiseSale, RestaurantAggregatorWiseSaleDTO>()
				.ForMember(des => des.OrderId, opt => opt.Condition(src => src.Order != null))
				.ForMember(des => des.RestaurantBalanceSheetId, opt => opt.Condition(src => src.RestaurantBalanceSheet.Id != 0))
				.ForMember(des => des.RestaurantAggregatorId, opt => opt.Condition(src => src.RestaurantAggregator.Id != 0))
				.ForMember(des => des.RestaurantAggregatorName, opt => opt.MapFrom(src => src.RestaurantAggregator != null ? src.RestaurantAggregator.Name : null))
				;
			CreateMap<RestaurantAggregatorWiseSaleDTO, RestaurantAggregatorWiseSale>();
		}
	}
}
