using AutoMapper;
using HelperClasses.DTOs.Order;
using WebAPI.Models;

namespace WebAPI.Mapper.OrderMapper
{
	public class OrderDetailOptionValueMapper : Profile
	{
		public OrderDetailOptionValueMapper()
		{
			CreateMap<OrderDetailOptionValue, OrderDetailOptionValueDTO>();
			CreateMap<OrderDetailOptionValueDTO, OrderDetailOptionValue>()
				.ForAllMembers(o => o.Condition((source, destination, member) => member != null));

			CreateMap<OrderItemOption, OrderDetailOptionValue>();
			CreateMap<OrderDetailOptionValue, OrderItemOption>();

			CreateMap<OrderDetailOptionValue, OrderDetailsOptionValuesDTO>();
			CreateMap<OrderDetailsOptionValuesDTO, OrderDetailOptionValue>();
		}
	}
}
