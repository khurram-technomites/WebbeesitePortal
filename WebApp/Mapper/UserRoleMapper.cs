using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class UserRoleMapper : Profile
    {
        public UserRoleMapper()
        {
            CreateMap<IdentityUserRoleDTO, IdentityUserRoleViewModel>();
            CreateMap<IdentityUserRoleViewModel, IdentityUserRoleDTO>();
        }
    }
}
