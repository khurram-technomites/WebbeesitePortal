using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartCMS
{
    public class SparePartMenuManagementDTO
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public long SparePartMenuId { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
        public SparePartsDealerDTO SparePartsDealer { get; set; }
        public SparePartMenuDTO SparePartMenu { get; set; }
    }
}
