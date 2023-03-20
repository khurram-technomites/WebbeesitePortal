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
    public class NotificationMapper : Profile
    {
        public NotificationMapper()
        {
            CreateMap<NotificationDTO, NotificationViewModel>();
            CreateMap<NotificationViewModel, NotificationDTO>();

            CreateMap<NotificationReceiverDTO, NotificationReceiverViewModel>();
            CreateMap<NotificationReceiverViewModel, NotificationReceiverDTO>();

            CreateMap<NotificationFilterDTO, NotificationFilterViewModel>();
            CreateMap<NotificationFilterViewModel, NotificationFilterDTO>();
        }
    }
}
