using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartCareerMapper : Profile
    {
        public SparePartCareerMapper()
        {
            CreateMap<SparePartCareerDTO, SparePartCareer>();
            CreateMap<SparePartCareer, SparePartCareerDTO>();
        }
    }
}
