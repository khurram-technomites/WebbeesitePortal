using HelperClasses.DTOs.NotificationFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface INotificationRecieverService
    {
        Task<IEnumerable<NotificationReceiver>> GetAllNotificationsByUser(NotificationFilterDTO Filter);
        Task<IEnumerable<NotificationReceiver>> GetNotificationById(long Id);
        Task<long> GetNewNotificationsCountByUser(string UserId, DateTime? StartDateTime = null, DateTime? EndDateTime = null);
        Task<NotificationReceiver> AddNotification(NotificationReceiver Entity);
        Task<NotificationReceiver> UpdateNotification(NotificationReceiver Entity);
        Task<IEnumerable<NotificationReceiver>> GetAllUnSeenNotificationsByUser(string UserId);
        Task<IEnumerable<NotificationReceiver>> MarkAllSeenByUser(string UserId);
        Task<NotificationReceiver> MarkSeenById(long Id);
        Task<NotificationReceiver> MarkReadById(long Id);
        Task<IEnumerable<NotificationReceiver>> MarkAllReadByUser(string UserId);
    }
}
