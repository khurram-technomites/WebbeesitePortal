using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant.Filter
{
    public class RestaurantFilter
    {
        public PagingParameters Paging { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int SortBy { get; set; }
    }
}
