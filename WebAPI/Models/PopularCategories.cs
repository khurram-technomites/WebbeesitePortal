using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    [Keyless]
    public class PopularCategories
    {
        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public decimal AvgPrice { get; set; }
    }
}
