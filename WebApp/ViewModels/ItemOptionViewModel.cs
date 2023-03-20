using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class ItemOptionViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public long ItemId { get; set; }
        public long RestaurantId { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPriceMain { get; set; }
        public bool IsRadioButton { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public ItemViewModel Item { get; set; }
        public List<ItemOptionValueViewModel> ItemOptionValues { get; set; }
    }
}
