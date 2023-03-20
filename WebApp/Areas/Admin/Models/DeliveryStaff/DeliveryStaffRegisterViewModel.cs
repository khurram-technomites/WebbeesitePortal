namespace WebApp.Areas.Admin.Models.DeliveryStaff
{
    public class DeliveryStaffRegisterViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public bool RequirePhoneNumberConfirmation { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Logo { get; set; }
        public DeliveryStaffViewModel DeliveryStaff { get; set; }
    }
}
