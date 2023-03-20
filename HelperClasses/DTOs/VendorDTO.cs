using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class VendorDTO
    {
        public VendorDTO()
        {
            VendorDocuments = new List<VendorDocumentDTO>();
        }
        public long Id { get; set; }

        public string NameAsPerTradeLicense { get; set; }
        public string NameArAsPerTradeLicense { get; set; }

        public string TradeLicenseNo { get; set; }
        public string FullName { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public string Whatsapp { get; set; }
        public string OfficeAddress { get; set; }
        public string Website { get; set; }
        public string UserId { get; set; }
        public long? CityId { get; set; }
        public long? CountryId { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }
        public string TrnNumber { get; set; }
        public string RejectionReason { get; set; }

        public DateTime CreationDate { get; set; }


        public AppUserDTO User { get; set; }
        public CityDTO City { get; set; }
        public CountryDTO Country { get; set; }

        public List<VendorDocumentDTO> VendorDocuments { get; set; }
    }
}
