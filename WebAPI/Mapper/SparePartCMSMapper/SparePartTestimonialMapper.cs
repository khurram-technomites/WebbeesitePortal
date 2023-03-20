using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartTestimonialMapper : Profile
    {
        public SparePartTestimonialMapper()
        {
            CreateMap<SparePartTestimonial, SparePartTestimonialDTO>();
            CreateMap<SparePartTestimonialDTO, SparePartTestimonial>();
        }
    }
}
