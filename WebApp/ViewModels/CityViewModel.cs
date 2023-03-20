using DocumentFormat.OpenXml.Drawing.Charts;
using Fingers10.ExcelExport.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class CityViewModel
    {
        public long Id { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "City Name")]
        [DisplayName("City Name")]
        public string Name { get; set; }
        [IncludeInReport(Order = 2)]
        [Display(Name = "City Name AR")]
        [DisplayName("City Name AR")]
        public string NameAr { get; set; }
        public long CountryId { get; set; }
        /*public string Logo { get; set; }*/
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        [NestedIncludeInReport]
        public CountryViewModel Country { get; set; }
    }
}
