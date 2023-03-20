using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.NotificationFilter
{
    public class NotificationFilterDTO
    {
        public string UserId { get; set; }
        public DateTime? StartDateTime { get; set; } = null;
        public DateTime? EndDateTime { get; set; } = null;
        public string Period { get; set; }
        public PagingParameters Paging { get; set; } = new PagingParameters();
    }
}
