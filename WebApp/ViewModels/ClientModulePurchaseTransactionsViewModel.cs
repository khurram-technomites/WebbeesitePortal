using System;

namespace WebApp.ViewModels
{
    public class ClientModulePurchaseTransactionsViewModel
    {
        public long Id { get; set; }
        public long ClientModulePurchaseID { get; set; }
        public decimal Amount { get; set; }
        public string NameOnCard { get; set; }
        public string MaskCardNo { get; set; }
        public string PaymentStatus { get; set; }
        public string InvoiceRef { get; set; }
        public DateTime CreationDate { get; set; }
        public ClientModulePurchasesViewModel ClientModulePurchases { get; set; }
    }
}
