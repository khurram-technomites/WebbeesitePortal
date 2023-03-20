using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantWaiterDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Contact2 { get; set; }
        public string Address { get; set; }
        public DateTime CreationDate { get; set; }

        public string Status { get; set; }
        public string Logo { get; set; }

        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public string RestaurantBranchName { get; set; }

        public RestaurantDTO Restaurant { get; set; }
        public RestaurantBranchDTO RestaurantBranch { get; set; }
    }
}
