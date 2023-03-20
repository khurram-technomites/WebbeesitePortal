using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CarModelDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAR { get; set; }
        public string Status { get; set; }
        public string Logo { get; set; }
        public long CarMakeId { get; set; }
        public DateTime CreationDate { get; set; }
        public CarMakeDTO CarMake { get; set; }
    }
}
