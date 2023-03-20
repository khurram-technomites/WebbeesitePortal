using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class TicketViewModel
    {
        public long Id { get; set; }
        public string TicketNo { get; set; }
        public long? RestaurantId { get; set; }
        public long? SupplierId { get; set; }
        public string UserId { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public DateTime CreationDate { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
        public UserViewModel user { get; set; }

        public List<TicketMessageViewModel> ticketConversation { get; set; }
    }
}
