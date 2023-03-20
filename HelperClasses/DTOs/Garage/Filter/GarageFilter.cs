using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage.Filter
{
    public class GarageFilter
    {
        public PagingParameters Paging { get; set; }
        public bool IsRecentRequired { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int SortBy { get; set; }
        public int CarMake { get; set; }
    }
}
