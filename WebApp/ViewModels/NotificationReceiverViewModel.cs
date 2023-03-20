using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class NotificationReceiverViewModel
    {
        public NotificationReceiverViewModel()
        {
            Notification = new NotificationViewModel();
        }
        public long Id { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverType { get; set; }
        public long NotificationId { get; set; }
        public bool IsSeen { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsRead { get; set; }
        public NotificationViewModel Notification { get; set; }
    }
}
