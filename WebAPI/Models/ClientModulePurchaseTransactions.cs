using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class ClientModulePurchaseTransactions:GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(ClientModulePurchases))]
        public long ClientModulePurchaseID { get; set; }
        public decimal Amount { get; set; }
        public string NameOnCard { get; set; }
        public string MaskCardNo { get; set; }
        public string PaymentStatus { get; set; }
        public string InvoiceRef { get; set; }
        public ClientModulePurchases ClientModulePurchases { get; set; }
    }
}
