using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.CustomerFeedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CustomerFeedbackMapper : Profile
    {
        public CustomerFeedbackMapper()
        {
            CreateMap<CustomerFeedbackViewModel, CustomerFeedbackDTO>();
            CreateMap<CustomerFeedbackDTO, CustomerFeedbackViewModel>();

            CreateMap<CustomerFeedbackReviewViewModel, CustomerFeedbackReviewDTO>();
            CreateMap<CustomerFeedbackReviewDTO, CustomerFeedbackReviewViewModel>();
        }
    }
}
