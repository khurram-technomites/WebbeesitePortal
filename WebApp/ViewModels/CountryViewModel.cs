using Fingers10.ExcelExport.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class CountryViewModel
    {
        public CountryViewModel()
        {
            Cities = new List<CityViewModel>();
        }
      
        public long Id { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "Country Name")]
        [DisplayName("Country Name")]
        public string Name { get; set; }
        [IncludeInReport(Order = 2)]
        [Display(Name = "Country Name AR")]
        [DisplayName("Country Name AR")]
        public string NameAr { get; set; }

        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        
        public List<CityViewModel> Cities { get; set; }
    }
}
