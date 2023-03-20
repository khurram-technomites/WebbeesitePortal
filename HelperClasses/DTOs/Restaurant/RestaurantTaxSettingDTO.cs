using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantTaxSettingDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal TAXPercent { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }


        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }

        public RestaurantDTO Restaurant { get; set; }
        public RestaurantBranchDTO RestaurantBranch { get; set; }
    }
}
