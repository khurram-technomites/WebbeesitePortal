using AutoMapper;
using HelperClasses.DTOs.Garage;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class SparePartRequestQuoteImageMapper : Profile
    {
        public SparePartRequestQuoteImageMapper()
        {
            CreateMap<SparePartRequestQuoteImageDTO, SparePartRequestQuoteImage>();
            CreateMap<SparePartRequestQuoteImage, SparePartRequestQuoteImageDTO>();

        }
    }
}
