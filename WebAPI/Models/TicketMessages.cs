using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class TicketMessages : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Ticket))]
        public long TicketId { get; set; }
        [ForeignKey(nameof(TicketDocument))]
        public long? TicketDocumentId { get; set; }
        public string SenderId { get; set; }
        public string SenderType { get; set; }
        public string Message { get; set; }
        public TicketDocument TicketDocument { get; set; }
        public Ticket Ticket { get; set; }
    }
}
