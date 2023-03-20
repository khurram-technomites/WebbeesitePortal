using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Mapper
{
    public class IdentityUserRoleMapper : Profile
    {
        public IdentityUserRoleMapper()
        {
            CreateMap<IdentityRole, IdentityUserRoleDTO>();
            CreateMap<IdentityUserRoleDTO, IdentityRole>();
        }
    }
}
