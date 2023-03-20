using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperClasses.DTOs.CardScheme;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantCardSchemeDTO
    {
        public long Id { get; set; }

        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public long CardSchemeId { get; set; }

        public RestaurantDTO Restaurant { get; set; }
        public RestaurantBranchDTO RestaurantBranch { get; set; }
        public CardSchemeDTO CardScheme { get; set; }
    }
}
