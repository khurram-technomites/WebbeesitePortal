using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses
{
    public class WebisteContactUsResponseDTO
    {
        public long GarageId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public string PhoneText { get; set; }
        public string EmailText { get; set; }
    }
}
