using AutoMapper;
using HelperClasses.DTOs;
using WebApp.Areas.Admin.Models;

namespace WebApp.Mapper
{
    public class CarModelMapper : Profile
    {
        public CarModelMapper()
        {
            CreateMap<CarModelViewModel, CarModelDTO>();
            CreateMap<CarModelDTO, CarModelViewModel>();
        }
    }
}
