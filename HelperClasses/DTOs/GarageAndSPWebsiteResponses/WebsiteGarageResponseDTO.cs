using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageAndSPWebsiteResponses
{
    public class WebsiteGarageResponseDTO
    {
        public bool IsServicesAllowed { get; set; }
        public bool IsBlogsAllowed { get; set; }
        public bool IsAppoinmnetsAllowed { get; set; }
        public bool IsTeamsAllowed { get; set; }
        public bool IsFeedbackAllowed { get; set; }
        public bool IsCareersAllowed { get; set; }
    }
}
