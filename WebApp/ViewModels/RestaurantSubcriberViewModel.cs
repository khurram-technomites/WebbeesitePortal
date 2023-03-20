using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantSubcriberViewModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public long RestaurantId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
