using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartExpertiseMapper : Profile
    {
        public SparePartExpertiseMapper()
        {
            CreateMap<SparePartExpertise, SparePartExpertiseDTO>();
            CreateMap<SparePartExpertiseDTO, SparePartExpertise>();
        }
    }
}
