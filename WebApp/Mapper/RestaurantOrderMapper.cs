using AutoMapper;
using HelperClasses.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantOrderMapper : Profile
    {
        public RestaurantOrderMapper()
        {
            CreateMap<OrderDTO, RestaurantOrderViewModel>();
            CreateMap<RestaurantOrderViewModel, OrderDTO>()
                .ForMember(x => x.RestaurantRatings, x => x.MapFrom(y => y.RestaurantRating));
            CreateMap<OrderDetailDTO, RestaurantOrderDetailViewModel>();
            CreateMap<RestaurantOrderDetailViewModel, OrderDetailDTO>();
            CreateMap<OrderDetailOptionValueViewModel, OrderDetailOptionValueDTO>();
            CreateMap<OrderDetailOptionValueDTO, OrderDetailOptionValueViewModel>();
        }
    }
}
