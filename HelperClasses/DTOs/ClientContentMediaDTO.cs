using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ClientContentMediaDTO
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string DocumentType { get; set; }

        public string DocumentPath { get; set; }
        public DateTime CreatedOn { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
