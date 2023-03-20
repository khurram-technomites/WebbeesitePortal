using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses
{
    public class WebsiteExpertiseResponseDTO
    {
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<string> Experties { get; set; }
    }
}
