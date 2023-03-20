using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartSubscriberMapper : Profile
    {
        public SparePartSubscriberMapper()
        {
            CreateMap<SparePartSubscriber, SparePartSubscriberDTO>();
            CreateMap<SparePartSubscriberDTO, SparePartSubscriber>();
        }
    }
}
