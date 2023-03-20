using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class MessageDTO
    {
        public string[] registration_ids { get; set; }
        public MessageNotificationDTO notification { get; set; }
        public object data { get; set; }
        public string sound { get; set; }

    }
    public class MessageNotificationDTO
    {
        public string title { get; set; }
        public string body { get; set; }
        public string sound { get; set; }
    }
}
