using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class SparePartRequestResponseDTO
    {
        public long Id { get; set; }
        public string SequenceNumber { get; set; }
        public string Title { get; set; }
        public string Decsription { get; set; }
        public string Condition { get; set; }
        public string ChasisNumber { get; set; }
        public string Address { get; set; }
        public string MulkiyaImageFront { get; set; }
        public string MulkiyaImageBack { get; set; }

        public CarMakeDTO CarMake { get; set; }
        public CarModelDTO CarModel { get; set; }
        public List<SparePartRequestImageDTO> Images { get; set; }
    }

    public class SparePartRequestQuotes
    {
        public long Id { get; set; }
        public string Condition { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal TejariPrice { get; set; }
    }
}
