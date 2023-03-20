using AutoMapper;
using HelperClasses.DTOs.ServiceStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.ServiceStaffMapper
{
    public class ServiceStaffMapper : Profile
    {
        public ServiceStaffMapper()
        {
            CreateMap<ServiceStaff, ServiceStaffDTO>();
            CreateMap<ServiceStaffDTO, ServiceStaff>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
