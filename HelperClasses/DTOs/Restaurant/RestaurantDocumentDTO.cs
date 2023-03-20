using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantDocumentDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string Path { get; set; }     
        public long ResturantId { get; set; }
    }
}
