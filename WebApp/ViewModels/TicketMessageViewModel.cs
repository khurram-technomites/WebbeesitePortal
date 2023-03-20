using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class TicketMessageViewModel
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public long? TicketDocumentId { get; set; }
        public string SenderId { get; set; }
        public string SenderType { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public TicketDocumentViewModel TicketDocument { get; set; } = new();
        public TicketViewModel Ticket { get; set; }
    }
}
