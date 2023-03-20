namespace WebApp.ViewModels
{
    public class ItemOptionValueViewModel
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string ValueAr { get; set; }
        public long ItemOptionId { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public ItemOptionViewModel ItemOption { get; set; }
    }
}
