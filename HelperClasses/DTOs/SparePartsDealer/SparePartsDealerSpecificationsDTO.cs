using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartsDealer
{
    public class SparePartsDealerSpecificationsDTO
    {
        public long Id { get; set; }
        public long SparePartsDealerId { get; set; }
        public long CarMakeId { get; set; }
        public long? CarModelId { get; set; }
        public CarMakeDTO CarMake { get; set; }
    }
}
