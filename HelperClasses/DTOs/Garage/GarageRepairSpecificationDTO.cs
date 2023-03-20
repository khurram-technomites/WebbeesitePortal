using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class GarageRepairSpecificationDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public long CarMakeId { get; set; }
        public string CarMakeName { get; set; }
        public string CarMakeLogo { get; set; }
        public long? CarModelId { get; set; }
        public CarMakeDTO CarMake { get; set; }
    }
}
