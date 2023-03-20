using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class PopularCategoriesDTO
    {
        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public decimal AvgPrice { get; set; }
    }
}
