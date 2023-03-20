using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageCustomerFeedbackMapper:Profile
    {
        public GarageCustomerFeedbackMapper()
        {
            CreateMap<GarageCustomerFeedback, GarageCustomerFeedbackDTO>();
            CreateMap<GarageCustomerFeedbackDTO, GarageCustomerFeedback>();
        }
    }
}
