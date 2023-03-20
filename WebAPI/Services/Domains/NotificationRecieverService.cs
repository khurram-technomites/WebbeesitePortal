using HelperClasses.DTOs.NotificationFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class NotificationRecieverService : INotificationRecieverService
    {
        private readonly INotificationRecieverRepo _repo;
        public NotificationRecieverService(INotificationRecieverRepo repo)
        {
            _repo = repo;
        }
        public async Task<NotificationReceiver> AddNotification(NotificationReceiver Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<IEnumerable<NotificationReceiver>> GetAllNotificationsByUser(NotificationFilterDTO Filter)
        {
            if (Filter.StartDateTime.HasValue && Filter.EndDateTime.HasValue)
                return await _repo.GetByIdAsync(x => x.ReceiverId == Filter.UserId && x.Notification.CreationDate >= Filter.StartDateTime && x.Notification.CreationDate <= Filter.EndDateTime,
                    ChildObjects: "Notification", Pagination: Filter.Paging, OrderExp: x => x.Id, IsOrderByDescending: true);
            else
                return await _repo.GetByIdAsync(x => x.ReceiverId == Filter.UserId, ChildObjects: "Notification", Pagination: Filter.Paging, OrderExp: x => x.Id, IsOrderByDescending: true);
        }

        public async Task<IEnumerable<NotificationReceiver>> GetAllUnSeenNotificationsByUser(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.ReceiverId == UserId && x.IsSeen == false, ChildObjects: "Notification");
        }

        public async Task<long> GetNewNotificationsCountByUser(string UserId, DateTime? StartDateTime = null, DateTime? EndDateTime = null)
        {
            if (StartDateTime.HasValue && EndDateTime.HasValue)
                return await _repo.GetCount(x => x.ReceiverId == UserId && x.IsRead == false && x.Notification.CreationDate >= StartDateTime && x.Notification.CreationDate <= EndDateTime, ChildObjects: "Notification");
            else
                return await _repo.GetCount(x => x.ReceiverId == UserId && x.IsRead == false);
        }

        public async Task<IEnumerable<NotificationReceiver>> GetNotificationById(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Notification");
        }

        public async Task<IEnumerable<NotificationReceiver>> MarkAllReadByUser(string UserId)
        {
            IEnumerable<NotificationReceiver> UnSeenNotifications = await _repo.GetByIdAsync(x => x.ReceiverId == UserId && x.IsRead == false);

            foreach (var Notification in UnSeenNotifications)
                Notification.IsRead = true;

            return await _repo.UpdateRangeAsync(UnSeenNotifications);
        }

        public async Task<IEnumerable<NotificationReceiver>> MarkAllSeenByUser(string UserId)
        {
            IEnumerable<NotificationReceiver> UnSeenNotifications = await _repo.GetByIdAsync(x => x.ReceiverId == UserId && x.IsSeen == false);

            foreach (var Notification in UnSeenNotifications)
                Notification.IsSeen = true;

            return await _repo.UpdateRangeAsync(UnSeenNotifications);
        }

        public async Task<NotificationReceiver> MarkReadById(long Id)
        {
            IEnumerable<NotificationReceiver> notification = await GetNotificationById(Id);

            notification.FirstOrDefault().IsRead = true;

            return await _repo.UpdateAsync(notification.FirstOrDefault());
        }

        public async Task<NotificationReceiver> MarkSeenById(long Id)
        {
            IEnumerable<NotificationReceiver> notification = await GetNotificationById(Id);

            notification.FirstOrDefault().IsSeen = true;

            return await _repo.UpdateAsync(notification.FirstOrDefault());
        }

        public async Task<NotificationReceiver> UpdateNotification(NotificationReceiver Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
