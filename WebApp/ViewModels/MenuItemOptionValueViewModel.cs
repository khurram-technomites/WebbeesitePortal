namespace WebApp.ViewModels
{
    public class MenuItemOptionValueViewModel
    {
		public long Id { get; set; }
		public string Value { get; set; }
		public string ValueAr { get; set; }
		public string Image { get; set; }
		public decimal Price { get; set; }
		public bool IsPriceMain { get; set; }
		public long MenuItemOptionId { get; set; }
		public MenuItemOptionViewModel MenuItemOption { get; set; }
	}
}
