using AutoMapper;
using HelperClasses.DTOs.Order;
using WebAPI.Models;

namespace WebAPI.Mapper.OrderMapper
{
    public class OrderDetailMapper : Profile
    {
        public OrderDetailMapper()
        {
            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<OrderDetailDTO, OrderDetail>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            CreateMap<OrderItem, OrderDetail>()
                .ForMember(x => x.OrderDetailOptionValues, x => x.MapFrom(y => y.OrderItemOptions));
            CreateMap<OrderDetail, OrderItem>()
                .ForMember(x => x.OrderItemOptions, x => x.MapFrom(y => y.OrderDetailOptionValues));
        }
    }
}
