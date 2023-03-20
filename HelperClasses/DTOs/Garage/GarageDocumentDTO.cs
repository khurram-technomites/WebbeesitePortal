using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class GarageDocumentDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string Path { get; set; }
        public long GarageId { get; set; }
    }
}
