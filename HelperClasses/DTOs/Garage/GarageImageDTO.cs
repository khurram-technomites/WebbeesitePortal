using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class GarageImageDTO
    {
        public long Id { get; set; }     
        public long GarageId { get; set; }
        public string Image { get; set; }
    }
}
