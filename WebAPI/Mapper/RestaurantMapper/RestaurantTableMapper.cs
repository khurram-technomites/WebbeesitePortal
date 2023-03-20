using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantTableMapper:Profile
    {
        public RestaurantTableMapper()
        {
            CreateMap<RestaurantTable,RestaurantTableDTO>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved) ? (src.RestaurantTableReservations.Any() ? src.RestaurantTableReservations.LastOrDefault().OrderId : null) : null))
                .ForMember(dest => dest.OrderNo, opt => opt.MapFrom(src => src.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved) ? (src.RestaurantTableReservations.Any() ? src.RestaurantTableReservations.LastOrDefault().Order.OrderNo : null) : null))
                //.ForMember(dest => dest.IsOrderGet, opt => opt.MapFrom(src => src.RestaurantTableReservations.Any() && (src.RestaurantTableReservations.LastOrDefault().Order != null ? true : false)))
                .ForMember(dest => dest.IsTableReserved, opt => opt.MapFrom(src => src.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved)))
                .ForMember(dest => dest.RestaurantTableReservations, opt => opt.Ignore())
                ;
            CreateMap<RestaurantTableDTO, RestaurantTable>();
        }
    }
}
