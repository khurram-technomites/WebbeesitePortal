using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantImagesViewModel
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public string Image { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
    }
}
