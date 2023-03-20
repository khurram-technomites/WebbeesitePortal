using AutoMapper;
using HelperClasses.DTOs.CustomerFeedback;
using WebAPI.Models;

namespace WebAPI.Mapper.CustomerFeedback
{
    public class CustomerFeedbackReviewOptionMapper : Profile
    {
        public CustomerFeedbackReviewOptionMapper()
        {
            CreateMap<CustomerFeedbackReviewOption, CustomerFeedbackReviewOptionDTO>();
            CreateMap<CustomerFeedbackReviewOptionDTO, CustomerFeedbackReviewOption>();

        }
    }
}
