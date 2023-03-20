using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartCMS
{
    public class SparePartCustomerFeedbackDTO
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string CustomerEmail { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }

        public SparePartsDealerDTO SparePartDealer { get; set; }
    }
}
