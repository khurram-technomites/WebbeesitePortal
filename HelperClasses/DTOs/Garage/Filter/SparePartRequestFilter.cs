using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage.Filter
{
    public class SparePartRequestFilter
    {
        public bool ActiveRequestsOnly { get; set; }
        public PagingParameters Paging { get; set; }
        public Sort? SortDate { get; set; }
        public StatusforGarageRequest? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
