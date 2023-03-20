using System;

namespace WebApp.ViewModels
{
    public class ModuleViewModel
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool ManageQunatity { get; set; }
        public long? Min { get; set; }
        public long? Max { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool IsSystem { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
