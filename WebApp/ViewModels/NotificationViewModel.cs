using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class NotificationViewModel
    {
        public NotificationViewModel()
        {
            NotificationReceivers = new List<NotificationReceiverViewModel>();
        }
        public long Id { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string OriginatorId { get; set; }
        public string OriginatorName { get; set; }
        public string OriginatorType { get; set; }
        public string Url { get; set; }
        public long RecordId { get; set; }
        public string Module { get; set; }
        public DateTime? CreationDate { get; set; }
        public List<NotificationReceiverViewModel> NotificationReceivers { get; set; }
        public List<int> Customers { get; set; }
    }
}
