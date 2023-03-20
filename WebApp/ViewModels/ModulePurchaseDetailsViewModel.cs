namespace WebApp.ViewModels
{
    public class ModulePurchaseDetailsViewModel
    {
        public long Id { get; set; }

        public long ClientModulePurchaseID { get; set; }

        public long ModuleID { get; set; }
        public long Quantity { get; set; }
        public decimal? TotalPrice { get; set; }

        public ClientModulePurchasesViewModel ClientModulePurchases { get; set; }
        public ModuleViewModel Module { get; set; }
    }
}
