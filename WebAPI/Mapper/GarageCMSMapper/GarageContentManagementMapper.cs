using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageContentManagementMapper : Profile
    {
        public GarageContentManagementMapper()
        {
            CreateMap<GarageContentManagementDTO, GarageContentManagement>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            CreateMap<GarageContentManagement, GarageContentManagementDTO>();
        }
    }
}
