using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class RestaurantCustomer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        [ForeignKey(nameof(RestaurantBranch))]
        public long RestaurantBranchId { get; set; }
        [ForeignKey(nameof(Customer))]
        public long CustomerId { get; set; }
        
        public Customer Customer { get; set; }
        public Restaurant Restaurant { get; set; }
        public RestaurantBranch RestaurantBranch { get; set; }

    }
}
