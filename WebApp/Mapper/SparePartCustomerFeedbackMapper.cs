using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartCustomerFeedbackMapper : Profile
    {
        public SparePartCustomerFeedbackMapper()
        {
            CreateMap<SparePartCustomerFeedbackDTO, SparePartCustomerFeedbackViewModel>();
            CreateMap<SparePartCustomerFeedbackViewModel, SparePartCustomerFeedbackDTO>();
        }
    }
}
