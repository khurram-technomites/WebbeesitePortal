using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartCustomerFeedbackMapper : Profile
    {
        public SparePartCustomerFeedbackMapper()
        {
            CreateMap<SparePartCustomerFeedback, SparePartCustomerFeedbackDTO>();
            CreateMap<SparePartCustomerFeedbackDTO, SparePartCustomerFeedback>();
        }
    }
}
