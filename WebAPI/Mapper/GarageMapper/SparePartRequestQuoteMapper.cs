using AutoMapper;
using HelperClasses.DTOs.Garage;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class SparePartRequestQuoteMapper:Profile
    {
        public SparePartRequestQuoteMapper()
        {
            CreateMap<SparePartRequestQuoteDTO, SparePartRequestQuote>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
            CreateMap<SparePartRequestQuote, SparePartRequestQuoteDTO>();
           
        }
    }
}
