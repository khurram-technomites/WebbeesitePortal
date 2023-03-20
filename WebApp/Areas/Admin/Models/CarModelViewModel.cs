using Fingers10.ExcelExport.Attributes;
using System;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Models
{
    public class CarModelViewModel
    {
        public long Id { get; set; }
        [IncludeInReport(Order = 1)]
        public string Name { get; set; }
        [IncludeInReport(Order = 2)]
        public string NameAR { get; set; }
        [IncludeInReport(Order = 3)]
        public string Status { get; set; }
        [IncludeInReport(Order = 4)]
        public string Logo { get; set; }

        public long CarMakeId { get; set; }
        public DateTime CreationDate { get; set; }

        public CarMakeViewModel CarMake { get; set; }
    }
}
