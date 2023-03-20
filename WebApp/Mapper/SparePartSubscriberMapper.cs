using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartSubscriberMapper : Profile
    {
        public SparePartSubscriberMapper()
        {
            CreateMap<SparePartSubscriberDTO, SparePartSubscriberViewModel>();
            CreateMap<SparePartSubscriberViewModel, SparePartSubscriberDTO>();
        }
    }
}
