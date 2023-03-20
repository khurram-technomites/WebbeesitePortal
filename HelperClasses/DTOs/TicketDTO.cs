using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class TicketDTO
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
        public RestaurantDTO Restaurant { get; set; }
        public SupplierDTO Supplier { get; set; }
        public UserDTO user { get; set; }
    }
}
