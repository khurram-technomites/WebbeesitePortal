using AutoMapper;
using HelperClasses.DTOs.CustomerFeedback;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class CustomerFeedbackMapper : Profile
    {
        public CustomerFeedbackMapper()
        {
            CreateMap<Models.CustomerFeedback, CustomerFeedbackDTO>();
            CreateMap<CustomerFeedbackDTO, Models.CustomerFeedback>();
        }
    }
}
