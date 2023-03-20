using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartRequestQuote : GeneralSchema
    {
        public SparePartRequestQuote()
        {
            SparePartRequestQuoteImages = new HashSet<SparePartRequestQuoteImage>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SparePartsDealer))]
        public long SparePartsDealerId { get; set; }
        [ForeignKey(nameof(SparePartRequest))]
        public long SparePartRequestId { get; set; }
        public string Image { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal TejariPrice { get; set; }
        public string Condition { get; set; }
        public decimal FougitoCommision { get; set; }
        public decimal DeliveryCharges { get; set; }
        public DateTime Warranty { get; set; }
        public decimal TotalPrice { get; set; }
        [MaxLength(7, ErrorMessage = "PaymentMethodPrefered length must be less than 7 characters")]
        public string PaymentMethodPrefered { get; set; }
        [MaxLength(4000, ErrorMessage = "Description length must be less than 4000 characters")]
        public string Description { get; set; }
        [MaxLength(4000, ErrorMessage = "DescriptionAr length must be less than 4000 characters")]
        public string DescriptionAr { get; set; }
        public bool IsAccepted { get; set; }
        public string Status { get; set; }

        public SparePartsDealer SparePartsDealer { get; set; }
        public SparePartRequest SparePartRequest { get; set; }
        public ICollection<SparePartRequestQuoteImage> SparePartRequestQuoteImages { get; set; }
    }
}
