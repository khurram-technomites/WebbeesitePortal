using AutoMapper;
using HelperClasses.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Mapper.OrderMapper
{
    public class OrderCardMapper : Profile
    {
        public OrderCardMapper()
        {
            CreateMap<Order, OrderCardDTO>();

            CreateMap<Order, OrderCardDetailsDTO>()
                .ForMember(x => x.RestaurantLogo, x => x.MapFrom(y => y.RestaurantBranch.Restaurant.Logo))
                .ForMember(x => x.RestaurantBranchName, x => x.MapFrom(y => y.RestaurantBranch.NameAsPerTradeLicense))
                .ForMember(x => x.Date, x => x.MapFrom(y => y.OrderPlacementDateTime.Value.ToString("g")))
                .ForMember(x => x.Address, x => x.MapFrom(y => y.Address))
                .ForMember(x => x.OrderNo, x => x.MapFrom(y => y.OrderNo))
                .ForMember(x => x.OrderAmount, x => x.MapFrom(y => y.TotalAmount))
                .ForMember(x => x.OrderType, x => x.MapFrom(y => y.DeliveryType))
                .ForMember(x => x.EstimatedDeliveryMinutes, x => x.MapFrom(y => (y.DeliveryDateTime.Value.AddMinutes(y.EstimatedDeliveryMinutes) - DateTime.UtcNow.ToDubaiDateTime()).Minutes));
        }
    }
}
