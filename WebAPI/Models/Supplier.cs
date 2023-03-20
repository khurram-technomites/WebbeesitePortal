using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Supplier : GeneralSchema
    {
        public Supplier()
        {
            SupplierDocuments = new HashSet<SupplierDocument>();
            SupplierItems = new HashSet<SupplierItem>();
            SupplierOrders = new HashSet<SupplierOrder>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(200, ErrorMessage = "NameAsPerTradeLicense must be less that 200 characters")]
        public string NameAsPerTradeLicense { get; set; }
        [MaxLength(20, ErrorMessage = "PhoneNumber must be less that 20 characters")]
        public string PhoneNumber { get; set; }
        [MaxLength(200, ErrorMessage = "Email must be less that 200 characters")]
        public string Email { get; set; }
        [MaxLength(20, ErrorMessage = "WhatsappNumber must be less that 20 characters")]
        public string WhatsappNumber { get; set; }
        public string Status { get; set; }
        public string ReferenceCode { get; set; }
        public string SupplierCode { get; set; }
        public string Description { get; set; }
        [MaxLength(200, ErrorMessage = "Bank must be less that 200 characters")]
        public string Bank { get; set; }
        [MaxLength(200, ErrorMessage = "BankAccountHolderName must be less that 200 characters")]
        public string BankAccountHolderName { get; set; }
        [MaxLength(200, ErrorMessage = "BankAccountNumber must be less that 200 characters")]
        public string BankAccountNumber { get; set; }
        public string IBAN { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Package))]
        public long? SupplierPackageId { get; set; }
        public DateTime? PackagePurchasingDatetime { get; set; }
        public DateTime? PackageExpiryDatetime { get; set; }
        [ForeignKey(nameof(Country))]
        public long? CountryId { get; set; }
        [ForeignKey(nameof(City))]
        public long? CityId { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal TaxPercentage { get; set; }
        public string Logo { get; set; }
        public AppUser User { get; set; }
        public SupplierPackage Package { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
        public ICollection<SupplierDocument> SupplierDocuments { get; set; }
        public ICollection<SupplierItem> SupplierItems { get; set; }
        public ICollection<SupplierOrder> SupplierOrders { get; set; }
    }
}
