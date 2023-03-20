using AutoMapper;
using HelperClasses.DTOs.DeliveryStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.DeliveryStaffMapper
{
    public class DeliveryStaffMapper : Profile
    {
        public DeliveryStaffMapper()
        {
            CreateMap<DeliveryStaff, DeliveryStaffDTO>();
            CreateMap<DeliveryStaffDTO, DeliveryStaff>();
        }
    }
}
