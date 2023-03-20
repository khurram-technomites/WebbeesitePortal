using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class AppUserMapper : Profile
    {
        public AppUserMapper()
        {
            CreateMap<AppUser, UserAuthData>()
                .ForMember(dest => dest.UserId,
                                opt => opt.MapFrom(x => x.Id))
                .ForMember(dest => dest.UserName,
                                opt => opt.MapFrom(x => x.Email));

            CreateMap<Claim, CustomeClaims>();

            CreateMap<AppUser, AppUserDTO>();
            CreateMap<AppUserDTO, AppUser>();
        }
    }
}
