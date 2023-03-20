using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageMenuMapper:Profile
    {
        public GarageMenuMapper()
        {
            CreateMap<GarageMenu, GarageMenuDTO>();
            CreateMap<GarageMenuDTO, GarageMenu>();
        }
    }
}
