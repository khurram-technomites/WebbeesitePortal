using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ClientModulePurchaseTransactionsDTO
    {
      
        public long Id { get; set; }
        public long ClientModulePurchaseID { get; set; }
        public decimal Amount { get; set; }
        public string NameOnCard { get; set; }
        public string MaskCardNo { get; set; }
        public string PaymentStatus { get; set; }
        public string InvoiceRef { get; set; }
        public DateTime CreationDate { get; set; }
        public ClientModulePurchasesDTO ClientModulePurchases { get; set; }
    }
}
