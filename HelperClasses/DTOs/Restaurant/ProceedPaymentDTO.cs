using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class ProceedPaymentDTO
    {
        public long OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public long? AggregatorId { get; set; }
        public long? CardId { get; set; }
        public long? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string PaidTo { get; set; }
        public decimal FinalBillAmount { get; set; } = 0;
        public decimal? CashReceived { get; set; } = 0;
        public decimal? CardAmount { get; set; } = 0;
        public decimal? Change { get; set; } = 0;
    }
}
