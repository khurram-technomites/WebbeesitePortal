using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CountryDTO
    {
        public CountryDTO()
        {
            Cities = new List<CityDTO>();
        }
        public DateTime CreationDate { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }

        public string Status { get; set; }
        public List<CityDTO> Cities { get; set; }
    }
}
