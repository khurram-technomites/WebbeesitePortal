using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Notification
    {
        public Notification()
        {
            NotificationReceivers = new HashSet<NotificationReceiver>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(225, ErrorMessage = "Title length must be less than 225 characters")]
        public string Title { get; set; }
        [MaxLength(225, ErrorMessage = "TitleAr length must be less than 225 characters")]
        public string TitleAr { get; set; }
        [MaxLength(500, ErrorMessage = "Description length must be less than 500 characters")]
        public string Description { get; set; }
        [MaxLength(500, ErrorMessage = "DescriptionAr length must be less than 500 characters")]
        public string DescriptionAr { get; set; }
        public string OriginatorId { get; set; }
        [MaxLength(100, ErrorMessage = "OriginatorName length must be less than 100 characters")]
        public string OriginatorName { get; set; }
        [MaxLength(50, ErrorMessage = "OriginatorType length must be less than 50 characters")]
        public string OriginatorType { get; set; }
        [MaxLength(1000, ErrorMessage = "Url length must be less than 1000 characters")]
        public string Url { get; set; }
        public long RecordId { get; set; }
        [MaxLength(50, ErrorMessage = "Module length must be less than 50 characters")]
        public string Module { get; set; }
        public DateTime? CreationDate { get; set; }
        public ICollection<NotificationReceiver> NotificationReceivers { get; set; }
    }
}
