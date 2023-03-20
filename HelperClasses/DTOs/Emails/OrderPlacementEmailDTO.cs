using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Emails
{
    public class OrderPlacementEmailDTO
    {
        public string Email { get; set; }
        public RestaurantDTO Restaurant { get; set; }
        public OrderDTO Order { get; set; }
    }
}
