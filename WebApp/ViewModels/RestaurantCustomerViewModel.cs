using System;

namespace WebApp.ViewModels
{
    public class RestaurantCustomerViewModel
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public long CustomerId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public CustomerViewModel Customer { get; set; }
    }
}
