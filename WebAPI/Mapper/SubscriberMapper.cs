using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class SubscriberMapper : Profile
    {
        public SubscriberMapper()
        {
            CreateMap<Subscriber, SubscriberDTO>();
            CreateMap<SubscriberDTO, Subscriber>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
