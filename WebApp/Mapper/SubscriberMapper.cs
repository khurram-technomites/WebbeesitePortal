using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SubscriberMapper : Profile
    {
        public SubscriberMapper()
        {
            CreateMap<SubscriberDTO, SubscriberViewModel>();
            CreateMap<SubscriberViewModel, SubscriberDTO>();
        }
    }
}
