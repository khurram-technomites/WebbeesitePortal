using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Customer : GeneralSchema
    {
        public Customer()
        {
            RestaurantCustomers = new HashSet<RestaurantCustomer>();
            Orders = new HashSet<Order>();
            TransactionHistories = new HashSet<CustomerTransactionHistory>();
            CustomerFavouriteBranches = new HashSet<CustomerFavouriteBranches>();
            CustomerAddresses = new HashSet<CustomerAddress>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Status  { get; set; }
        public AppUser User  { get; set; }
        public ICollection<RestaurantCustomer> RestaurantCustomers { get; set; }
        public ICollection<CustomerCoupon> CustomerCoupons { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<CustomerTransactionHistory> TransactionHistories { get; set; }
        public ICollection<CustomerFavouriteBranches> CustomerFavouriteBranches { get; set; }
        public ICollection<CustomerAddress> CustomerAddresses { get; set; }
    }
}
