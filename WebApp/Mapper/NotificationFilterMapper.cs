using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.NotificationFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class NotificationFilterMapper : Profile
    {
        public NotificationFilterMapper()
        {
            CreateMap<NotificationFilterDTO, NotificationFilterViewModel>();
            CreateMap<NotificationFilterViewModel, NotificationFilterDTO>();
        }
    }
}
