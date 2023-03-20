using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantImagesDTO
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public string Image { get; set; }
        public RestaurantDTO Restaurant { get; set; }
    }
}
