using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartMenuMapper : Profile
    {
        public SparePartMenuMapper()
        {
            CreateMap<SparePartMenu, SparePartMenuDTO>();
            CreateMap<SparePartMenuDTO, SparePartMenu>();
        }
    }
}
