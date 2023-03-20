using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class AreaDTO
    {
        public long Id { get; set; }     
        public string Name { get; set; }     
        public string NameAr { get; set; }
        public long CityId { get; set; }
        public DateTime CreationDate { get; set; }
        public CityDTO City { get; set; }
    }
}
