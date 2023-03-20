using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class DuplicateMenuItemOptionViewModel
    {
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPriceMain { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public List<DuplicateMenuItemOptionValueViewModel> MenuItemOptionValues { get; set; }
    }
}
