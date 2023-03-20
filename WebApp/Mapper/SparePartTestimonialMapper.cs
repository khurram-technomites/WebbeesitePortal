using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartTestimonialMapper : Profile
    {
        public SparePartTestimonialMapper()
        {
            CreateMap<SparePartTestimonialDTO, SparePartTestimonialViewModel>();
            CreateMap<SparePartTestimonialViewModel, SparePartTestimonialDTO>();
        }
    }
}
