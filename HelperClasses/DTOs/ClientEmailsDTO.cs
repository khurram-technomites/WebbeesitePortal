using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ClientEmailsDTO
    {
       
        public long Id { get; set; }
       
        public long ClientId { get; set; }

       
        public string Email { get; set; }


        public GarageDTO Garage { get; set; }
    }
}
