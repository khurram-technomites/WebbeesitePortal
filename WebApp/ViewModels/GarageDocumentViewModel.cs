using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class GarageDocumentViewModel
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string FormattedDate { get { return ExpiryDateTime.ToShortDateString(); } set { } }
        public string Path { get; set; }
        public long GarageId { get; set; }
    }
}
