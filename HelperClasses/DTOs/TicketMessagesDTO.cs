using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class TicketMessagesDTO
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public long? TicketDocumentId { get; set; }
        public string SenderId { get; set; }
        public string SenderType { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public TicketDocumentDTO TicketDocument { get; set; }
        public TicketDTO Ticket { get; set; }

    }
}
