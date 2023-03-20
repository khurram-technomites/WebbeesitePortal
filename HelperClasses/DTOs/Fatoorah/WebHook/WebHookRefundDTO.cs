using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Fatoorah.WebHook
{
    public class WebHookRefundDTO
    {
        public string RefundReference { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RefundStatus { get; set; }
        public decimal Amount { get; set; }
        public string Comments { get; set; }
        public long InvoiceId { get; set; }
    }
}
