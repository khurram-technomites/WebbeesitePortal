using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class NotificationFilterViewModel
    {
        public string UserId { get; set; }
        public PagingParameters Paging { get; set; }
    }
}
