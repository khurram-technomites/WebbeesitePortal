using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ClientIndustriesDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
