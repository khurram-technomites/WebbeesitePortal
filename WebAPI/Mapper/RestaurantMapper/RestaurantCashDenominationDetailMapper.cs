using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantCashDenominationDetailMapper:Profile
    {
        public RestaurantCashDenominationDetailMapper()
        {
            CreateMap<RestaurantCashDenominationDetail, RestaurantCashDenominationDetailDTO>()
                .ForMember(des => des.CurrencyNoteName, opt => opt.MapFrom(src => src.CurrencyNote != null ? src.CurrencyNote.Name : ""));
            CreateMap<RestaurantCashDenominationDetailDTO, RestaurantCashDenominationDetail>();
        }
    }
}
