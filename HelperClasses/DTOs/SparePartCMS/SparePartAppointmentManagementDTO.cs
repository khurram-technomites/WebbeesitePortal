using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartCMS
{
    public class SparePartAppointmentManagementDTO
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Section01 { get; set; }
        public string Section02 { get; set; }
        public string Section03 { get; set; }
        public string Section04 { get; set; }
        public DateTime CreationDate { get; set; }
        public SparePartsDealerDTO SparePartDealer { get; set; }

    }
}
