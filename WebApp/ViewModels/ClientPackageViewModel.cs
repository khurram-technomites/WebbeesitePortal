using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class ClientPackageViewModel
    {
        public List<ModuleViewModel> Modules { get; set; }= new List<ModuleViewModel>();
        public ClientModulePurchasesViewModel ClientModulePurchases { get; set; } = new ClientModulePurchasesViewModel();
        public List<ModulePurchaseDetailsViewModel> ModulePurchaseDetails { get; set; } = new List<ModulePurchaseDetailsViewModel>();
        public List<ClientModulesViewModel> ClientModules { get; set; } = new List<ClientModulesViewModel>();
    }
}
