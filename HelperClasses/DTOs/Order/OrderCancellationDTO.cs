using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Order
{
    public class OrderCancellationDTO
    {
        [Required]
        public long OrderId { get; set; }
        [Required]
        public string CancellationReason { get; set; }
    }
}
