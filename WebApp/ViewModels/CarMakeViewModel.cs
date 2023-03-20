using Fingers10.ExcelExport.Attributes;
using System;

namespace WebApp.ViewModels
{
    public class CarMakeViewModel
    {
        public long Id { get; set; }
        [IncludeInReport(Order = 1)]
        public string Name { get; set; }
        public string NameAr { get; set; }
        [IncludeInReport(Order = 2)]
        public string Status { get; set; }

        public string Logo { get; set; }
        [IncludeInReport(Order = 3)]
        public DateTime CreationDate { get; set; }
    }
}
