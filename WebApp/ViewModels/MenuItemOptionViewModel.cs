using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class MenuItemOptionViewModel
    {
        public long Id { get; set; }
        public long MenuItemId { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPriceMain { get; set; }
        public bool IsRadioButton { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public List<MenuItemOptionValueViewModel> MenuItemOptionValues { get; set; }
    }
}
