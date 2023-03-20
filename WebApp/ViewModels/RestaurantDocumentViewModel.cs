using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantDocumentViewModel
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string FormattedDate { get { return ExpiryDateTime.ToShortDateString(); } set { } }
        public string Path { get; set; }
        public long ResturantId { get; set; }
    }
}
