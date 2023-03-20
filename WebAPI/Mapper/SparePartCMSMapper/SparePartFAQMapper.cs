using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartFAQMapper : Profile
    {
        public SparePartFAQMapper()
        {
            CreateMap<SparePartFAQDTO, SparePartFAQ>();
            CreateMap<SparePartFAQ, SparePartFAQDTO>();
        }
    }
}
