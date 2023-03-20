using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class NotificationMapper : Profile
    {
        public NotificationMapper()
        {
            CreateMap<Notification, NotificationDTO>();
            CreateMap<NotificationDTO, Notification>();

            CreateMap<NotificationReceiver, NotificationReceiverDTO>();
            CreateMap<NotificationReceiverDTO, NotificationReceiver>();
        }
    }
}
