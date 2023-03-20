using WebApp.Areas.Admin.Models.DeliveryStaff;

namespace WebApp.Areas.Admin.Models
{
    public class ServiceAndDeliveryStaffRegisteredViewModel
    {
        public string RegisteringFor { get; set; }
        public ServiceStaffRegisterViewModel ServiceStaffRegister { get; set; }
        public DeliveryStaffRegisterViewModel DeliveryStaffRegister { get; set; }
    }
}
