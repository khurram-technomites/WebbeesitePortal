using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartRequest : GeneralSchema
    {
        public SparePartRequest()
        {
            SparePartRequestQuotes = new HashSet<SparePartRequestQuote>();
            SparePartRequestImages = new HashSet<SparePartRequestImage>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(CarMake))]
        public long CarMakeId { get; set; }
        [ForeignKey(nameof(CarModel))]
        public long CarModelId { get; set; }
        public int? BuildYear { get; set; }
        [MaxLength(50, ErrorMessage = "SparePart length must be less than 50 characters")]
        public string Title { get; set; }
        [MaxLength(50, ErrorMessage = "SparePartAr length must be less than 50 characters")]
        public string TitletAr { get; set; }
        [MaxLength(4000, ErrorMessage = "Description length must be less than 4000 characters")]
        public string Description { get; set; }
        [MaxLength(4000, ErrorMessage = "DescriptionAr length must be less than 4000 characters")]
        public string DescriptionAr { get; set; }
        [MaxLength(5, ErrorMessage = "Condition length must be less than 5 characters")]
        public string Condition { get; set; }
        [MaxLength(10, ErrorMessage = "Status length must be less than 10 characters")]
        public string Status { get; set; }
        [MaxLength(100, ErrorMessage = "ChasisNumber length must be less than 100 characters")]
        public string ChasisNumber { get; set; }
        public string MulkiyaImageFront { get; set; }
        public string MulkiyaImageBack { get; set; }
        public long? SparePartRequestQuoteId { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public decimal Price { get; set; }
        public decimal FougitoCommision { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal TotalPrice { get; set; }
        [MaxLength(7, ErrorMessage = "PaymentMethod length must be less than 7 characters")]
        public string PaymentMethod { get; set; }
        [MaxLength(250, ErrorMessage = "Address length must be less than 250 characters")]
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string SequenceNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public CarMake CarMake { get; set; }
        public CarModel CarModel { get; set; }
        public Garage Garage { get; set; }
        public ICollection<SparePartRequestQuote> SparePartRequestQuotes { get; set; }
        public ICollection<SparePartRequestImage> SparePartRequestImages { get; set; }
    }
}
