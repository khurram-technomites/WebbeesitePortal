using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageCustomerFeedbackDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string CustomerEmail { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
