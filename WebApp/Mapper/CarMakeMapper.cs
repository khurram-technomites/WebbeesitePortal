using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CarMakeMapper : Profile
    {
        public CarMakeMapper()
        {
            CreateMap<CarMakeViewModel, CarMakeDTO>();
            CreateMap<CarMakeDTO, CarMakeViewModel>();
        }
    }
}
