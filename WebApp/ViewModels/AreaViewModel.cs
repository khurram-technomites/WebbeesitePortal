using Fingers10.ExcelExport.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class AreaViewModel
    {
        public long Id { get; set; }
        [IncludeInReport(Order = 1)]
        public string Name { get; set; }
        [IncludeInReport(Order = 2)]
        public string NameAr { get; set; }
        public long CityId { get; set; }
        /*public string Logo { get; set; }*/
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        [NestedIncludeInReport]
        public CityViewModel City { get; set; }
    }
}
