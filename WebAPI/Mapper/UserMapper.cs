using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<AppUser, UserDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.Id));

            CreateMap<UserDTO, AppUser>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.UserId));
        }
    }
}
