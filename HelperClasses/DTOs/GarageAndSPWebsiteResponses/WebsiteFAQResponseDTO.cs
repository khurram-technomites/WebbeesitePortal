using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses
{
    public class WebsiteFAQResponseDTO
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Position { get; set; }
    }
}
