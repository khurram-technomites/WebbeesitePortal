using System;

namespace HelperClasses.DTOs.DeliveryStaff
{
    public class DeliveryStaffRegisterDTO
    {
        public DeliveryStaffRegisterDTO()
        {
            DeliveryStaff = new DeliveryStaffDTO();
        }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public bool RequirePhoneNumberConfirmation { get; set; }
        public string PasswordHash { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public DeliveryStaffDTO DeliveryStaff { get; set; }
    }
}
