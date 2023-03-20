using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantCustomerDTO
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public long CustomerId { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}
