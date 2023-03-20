using HelperClasses.DTOs.DeliveryStaff;
using HelperClasses.DTOs.ServiceStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.ServiceAndDeliveryStaffDTO
{
    public class ServiceAndDeliveryStaffRegisterDTO
    {
        public string RegisteringFor { get; set; }
        public ServiceStaffRegisterDTO ServiceStaffRegister { get; set; }
        public DeliveryStaffRegisterDTO DeliveryStaffRegister { get; set; }

        public ServiceAndDeliveryStaffRegisterDTO()
        {
            ServiceStaffRegister = new ServiceStaffRegisterDTO();
            DeliveryStaffRegister = new DeliveryStaffRegisterDTO();
        }
    }
}
