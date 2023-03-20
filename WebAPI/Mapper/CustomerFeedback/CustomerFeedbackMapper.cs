using AutoMapper;
using HelperClasses.DTOs.CustomerFeedback;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;
using System.Threading.Tasks;

namespace WebAPI.Mapper.CustomerFeedbackMapper
{
    public class CustomerFeedbackMapper : Profile
    {
        public CustomerFeedbackMapper()
        {
            CreateMap<Models.CustomerFeedback, CustomerFeedbackDTO>();
            CreateMap<CustomerFeedbackDTO, Models.CustomerFeedback>()
                .ForAllMembers(x => x.Condition((source, destination, member) => member != null))
                ;
        }
    }
}
