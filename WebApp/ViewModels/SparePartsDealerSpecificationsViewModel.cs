using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SparePartsDealerSpecificationsViewModel
    {
        public long Id { get; set; }
        public long SparePartsDealerId { get; set; }
        public long CarMakeId { get; set; }
        public long? CarModelId { get; set; }
        public CarMakeViewModel CarMake { get; set; }
    }
}
