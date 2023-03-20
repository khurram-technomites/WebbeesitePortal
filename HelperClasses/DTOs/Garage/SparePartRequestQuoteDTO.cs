using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperClasses.DTOs.SparePartsDealer;

namespace HelperClasses.DTOs.Garage
{
    public class SparePartRequestQuoteDTO
    {
        public SparePartRequestQuoteDTO()
        {
            SparePartRequestQuoteImages= new List<SparePartRequestQuoteImageDTO>();
        }
        public long Id { get; set; }
        public long SparePartsDealerId { get; set; }
        public long SparePartRequestId { get; set; }
        public string Image { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? TejariPrice { get; set; }
        public string Condition { get; set; }
        public decimal FougitoCommision { get; set; }
        public decimal DeliveryCharges { get; set; }
        public DateTime Warranty { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethodPrefered { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public bool IsAccepted { get; set; }
        public string Status { get; set; }
        public string OffersCount { get; set; }
        public DateTime CreationDate { get; set; }
        public SparePartsDealerDTO SparePartsDealer { get; set; }
        public SparePartRequestDTO SparePartRequest { get; set; }
        public List<SparePartRequestQuoteImageDTO> SparePartRequestQuoteImages { get; set; }
    }
}
