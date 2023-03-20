using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class CityViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public DateTime CreationDate { get; set; }
        public long CountryId { get; set; }
    }
}
