using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SubscriberViewModel
    {
        public long Id { get; set; }
        public string Email { get; set; }

        public DateTime CreationDate { get; set; }

        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
