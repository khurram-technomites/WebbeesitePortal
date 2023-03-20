using System;

namespace HelperClasses.DTOs.ServiceStaff
{
    public class ServiceStaffRegisterDTO
    {
        public ServiceStaffRegisterDTO()
        {
            ServiceStaff = new ServiceStaffDTO();
        }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public bool RequirePhoneNumberConfirmation { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public ServiceStaffDTO ServiceStaff { get; set; }
    }
}
