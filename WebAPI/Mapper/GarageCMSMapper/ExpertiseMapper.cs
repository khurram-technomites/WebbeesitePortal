using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class ExpertiseMapper:Profile
    {
        public ExpertiseMapper()
        {
            CreateMap<Expertise, ExpertiseDTO>();
            CreateMap<ExpertiseDTO, Expertise>();
        }
    }
}
