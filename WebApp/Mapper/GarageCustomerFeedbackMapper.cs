using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageCustomerFeedbackMapper:Profile
    {
        public GarageCustomerFeedbackMapper()
        {
            CreateMap<GarageCustomerFeedbackViewModel, GarageCustomerFeedbackDTO>();
            CreateMap<GarageCustomerFeedbackDTO, GarageCustomerFeedbackViewModel>();
        }
    }
}
