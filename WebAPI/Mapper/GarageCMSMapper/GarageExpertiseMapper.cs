using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageExpertiseMapper:Profile
    {
        public GarageExpertiseMapper()
        {
            CreateMap<GarageExpertiseDTO, GarageExpertise>();
            CreateMap<GarageExpertise, GarageExpertiseDTO>();
        }
    }
}
