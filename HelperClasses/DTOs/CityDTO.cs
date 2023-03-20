using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CityDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public long CountryId { get; set; }
        public string Logo { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public CountryDTO Country { get; set; }
    }
}
