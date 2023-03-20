using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageTestimonialsMapper:Profile
    {
        public GarageTestimonialsMapper()
        {
            CreateMap<GarageTestimonialsDTO, GarageTestimonials>();
            CreateMap<GarageTestimonials, GarageTestimonialsDTO>();
        }
    }
}
