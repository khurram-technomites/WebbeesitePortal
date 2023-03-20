using System;

namespace WebApp.ViewModels
{
    public class ClientModulePurchasesViewModel
    {
        public long Id { get; set; }

        public long GarageID { get; set; }

        public decimal CouponDiscountAmount { get; set; }

        public string CouponCode { get; set; }

        public decimal CouponDiscountPercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public decimal AmountToBePaid { get; set; }
        public decimal SubTotal { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentRef { get; set; }
        public string PaymentInvoiceID { get; set; }
        public string PaymentUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
