using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }
        public string NameAsPerTradeLicense { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WhatsappNumber { get; set; }
        public string Status { get; set; }
        public string ReferenceCode { get; set; }
        public string SupplierCode { get; set; }
        public string Description { get; set; }
        public string Bank { get; set; }
        public string BankAccountHolderName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IBAN { get; set; }
        public string UserId { get; set; }
        public long CountryId { get; set; }
        public long CityId { get; set; }
        public string Address { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Logo { get; set; }
        public DateTime CreationDate { get; set; }
        public long? SupplierPackageId { get; set; }
        public DateTime? PackagePurchasingDatetime { get; set; }
        public DateTime? PackageExpiryDatetime { get; set; }
        public AppUserViewModel User { get; set; }
        public List<SupplierDocumentViewModel> SupplierDocuments { get; set; }
        public List<SupplierItemViewModel> SupplierItems { get; set; }
    }
    public class SupplierOBj
    {

        public SupplierViewModel supplier { get; set; }
        public IEnumerable<SupplierViewModel> supplierList { get; set; }

    }
}
