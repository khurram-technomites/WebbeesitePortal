using HelperClasses.DTOs;
using HelperClasses.DTOs.NotificationFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface INotificationClient
    {
        Task<IEnumerable<NotificationReceiverDTO>> GetNotification(NotificationFilterDTO model);
        Task<IEnumerable<NotificationReceiverDTO>> MarkNotificationsAsSeen(String UserId);
        Task<NotificationReceiverDTO> MarkNotificationsAsRead(long NotificationId);
        Task<NotificationDTO> GetNotificationByID(long Id);
        Task<NotificationDTO> Create(NotificationViewModel model);
        Task<NotificationDTO> Edit(NotificationDTO model);

        Task<NotificationDTO> Delete(long Id);
    }
}
