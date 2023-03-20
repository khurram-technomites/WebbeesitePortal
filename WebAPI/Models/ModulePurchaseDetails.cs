using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class ModulePurchaseDetails : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(ClientModulePurchases))]
        public long ClientModulePurchaseID { get; set; }
        [ForeignKey(nameof(Module))]
        public long ModuleID { get; set; }
        public long Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public ClientModulePurchases ClientModulePurchases { get; set; }
        public Module Module { get; set; }
    }
}
