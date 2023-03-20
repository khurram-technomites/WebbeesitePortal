using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ClientDomainSuggestionsDTO
    {
       
        public long Id { get; set; }
      
        public long ClientId { get; set; }


        public string Domain { get; set; }

        public int Position { get; set; }

        public GarageDTO Garage { get; set; }
    }
}
