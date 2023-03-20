using System;
namespace WebApp.ViewModels
{
    public class ClientModulesViewModel
    {
        public long Id { get; set; }

        public long ClientId { get; set; }

        public long ModuleId { get; set; }

        public long Quantity { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }

        public GarageViewModel Garage { get; set; }

        public ModuleViewModel Module { get; set; }
    }
}
