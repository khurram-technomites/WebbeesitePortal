using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Emails
{
    public class GeneralEmailDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string HTMLBody { get; set; }
        public string ButtonLink { get; set; }
        public string ButtonText{ get; set; }

        public RestaurantDTO Restaurant { get; set; }
    }
}
