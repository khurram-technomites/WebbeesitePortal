using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageSubscribersMapper:Profile
    {
        public GarageSubscribersMapper()
        {
            CreateMap<GarageSubscribers, GarageSubscribersDTO>();
            CreateMap<GarageSubscribersDTO, GarageSubscribers>();
        }
    }
}
