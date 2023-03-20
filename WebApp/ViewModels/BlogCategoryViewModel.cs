using System;

namespace WebApp.ViewModels
{
    public class BlogCategoryViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Module { get; set; }
        public long GarageId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
