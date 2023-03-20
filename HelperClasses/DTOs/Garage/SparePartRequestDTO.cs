using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class SparePartRequestDTO
    {
        public SparePartRequestDTO()
        {
            SparePartRequestImages = new List<SparePartRequestImageDTO>();
        }
        public long Id { get; set; }
        public long CarMakeId { get; set; }
        public string CarMakeName { get; set; }
        public long CarModelId { get; set; }
        public string CarModelName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int? BuildYear { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public string ChasisNumber { get; set; }
        public string MulkiyaImageFront { get; set; }
        public string MulkiyaImageBack { get; set; }
        public long GarageId { get; set; }       
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string SequenceNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public long? SparePartRequestQuoteId { get; set; }
        public float OffersCount { get; set; }

        public List<SparePartRequestImageDTO> SparePartRequestImages { get; set; }

        public List<SparePartRequestQuoteDTO> SparePartRequestQuotes { get; set; }
    }
}
