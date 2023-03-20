using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class NotificationReceiverDTO
    {
        public NotificationReceiverDTO()
        {
            Notification = new NotificationDTO();
        }
        public long Id { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverType { get; set; }
        public long NotificationId { get; set; }
        public bool IsSeen { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsRead { get; set; }
        public NotificationDTO Notification { get; set; }
    }
}
