using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class GarageCustomerInvoiceDTO
    {
        public long Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public string Description { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public string NameOnCard { get; set; }
        public string InvoiceNo { get; set; }
        public string MaskCardNo { get; set; }
        public long OrderId { get; set; }
        public long? CustomerId { get; set; }
        public string PaymentId { get; set; }
        public string Origin { get; set; }
        public bool IsPaid { get; set; }
        public string InvoiceRef { get; set; }
        public bool PaymentRef { get; set; }
        public bool PaymentCaptured { get; set; }
        public long GarageId { get; set; }

        public DateTime CreationDate { get; set; }
        public  decimal TotalInvoicePrice { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
