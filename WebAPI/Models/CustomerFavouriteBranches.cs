using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CustomerFavouriteBranches
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Customer))]
        public long CustomerId { get; set; }
        [ForeignKey(nameof(RestaurantBranch))]
        public long BranchId { get; set; }
        public Customer Customer { get; set; }
        public RestaurantBranch RestaurantBranch { get; set; }
    }
}
