using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
   public class TicketDocumentDTO
    {
        public long Id { get; set; }

        public string URL { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
