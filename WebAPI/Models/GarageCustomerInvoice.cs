using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageCustomerInvoice : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50, ErrorMessage = "InvoiceNo must be less than 50 characters")]
        public string InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public string Description { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public string NameOnCard { get; set; }
        public string MaskCardNo { get; set; }
        public long OrderId { get; set; }
        public long? CustomerId { get; set; }
        public string PaymentId { get; set; }
        public string Origin { get; set; }
        public bool IsPaid { get; set; }
        [MaxLength(20, ErrorMessage = "InvoiceRef must be less than 20 characters")]
        public string InvoiceRef { get; set; }
        public bool PaymentRef { get; set; }
        public bool PaymentCaptured { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }

        public Garage Garage { get; set; }
    }
}
