using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartsDealer.Filter
{
    public class SparePartFilterDTO
    {
        public PagingParameters Paging { get; set; }
        public bool IsRecentRequired { get; set; }
    }
}
