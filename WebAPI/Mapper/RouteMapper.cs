using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class RouteMapper : Profile
    {
        public RouteMapper()
        {
            CreateMap<RouteGroup, RouteGroupDTO>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(x => x.Group.GroupName))
                .ForMember(dest => dest.RoutePath, opt => opt.MapFrom(x => x.Route.RoutePath));

            CreateMap<RouteGroupDTO, RouteGroup>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(x => x.GroupId))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(x => x.RouteId));

            CreateMap<Group, GroupDTO>();
            CreateMap<GroupDTO, Group>();
        }
    }
}
