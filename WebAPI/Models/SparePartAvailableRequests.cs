using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartAvailableRequests
    {
        public long SparePartsDealerId { get; set; }
        public long SparePartRequestId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string SequenceNumber { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
