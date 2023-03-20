using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class TrendingItem
    {
        public string Name { get; set; }
        public long RestaurantId { get; set; }
        public long Count { get; set; }
    }
}
