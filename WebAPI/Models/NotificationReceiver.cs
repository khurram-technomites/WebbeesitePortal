using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class NotificationReceiver
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ReceiverId { get; set; }

        [MaxLength(50,ErrorMessage = "Receiver Type length must be less than 50 characters")]
        public string ReceiverType { get; set; }
        [ForeignKey(nameof(Notification))]
        public long NotificationId { get; set; }
        public bool IsSeen { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsRead { get; set; }
        public Notification Notification { get; set; }

    }
}
