using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageBlogMapper:Profile
    {
        public GarageBlogMapper()
        {
            CreateMap<GarageBlogDTO, GarageBlog>();
            CreateMap<GarageBlog, GarageBlogDTO>();
        }
    }
}
