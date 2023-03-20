using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageCustomerAppointmentDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string CustomerComments { get; set; }
        public string Status { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
