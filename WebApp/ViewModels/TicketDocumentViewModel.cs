using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class TicketDocumentViewModel
    {
        public long Id { get; set; }

        public string URL { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
