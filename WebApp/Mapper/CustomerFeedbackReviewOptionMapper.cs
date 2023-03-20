using AutoMapper;
using HelperClasses.DTOs.CustomerFeedback;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CustomerFeedbackReviewOptionMapper : Profile
    {
        public CustomerFeedbackReviewOptionMapper()
        {
            CreateMap<CustomerFeedbackReviewOptionDTO, CustomerFeedbackReviewOptionViewModel>();
            CreateMap<CustomerFeedbackReviewOptionViewModel, CustomerFeedbackReviewOptionDTO>();
        }
    }
}
