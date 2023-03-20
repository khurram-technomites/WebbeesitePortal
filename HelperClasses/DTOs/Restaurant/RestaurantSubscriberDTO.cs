using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantSubscriberDTO
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public long RestaurantId { get; set; }
    }
}
