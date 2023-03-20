using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CustomerTransactionHistory : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string NameOnCard { get; set; }
        public string MaskCardNo { get; set; }
        public string TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        public long OrderId { get; set; }
        public long? CustomerId { get; set; }
        public string PaymentId { get; set; }
        public string Origin { get; set; }

        public Order Order { get; set; }
        public Customer Customer { get; set; }

    }
}
