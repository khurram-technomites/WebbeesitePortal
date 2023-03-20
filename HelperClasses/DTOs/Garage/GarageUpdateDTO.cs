using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class GarageUpdateDTO
    {
        public int Id { get; set; }
        public string Logo { get; set; }
        public string NameAsPerTradeLicense { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonEmail { get; set; }
    }
}
