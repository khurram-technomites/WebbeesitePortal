using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageCareerMapper:Profile
    {
        public GarageCareerMapper()
        {
            CreateMap<GarageCareers, GarageCareerDTO>();
            CreateMap<GarageCareerDTO, GarageCareers>();
        }
    }
}
