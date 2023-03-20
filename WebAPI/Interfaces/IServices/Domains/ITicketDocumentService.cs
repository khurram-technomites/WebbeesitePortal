using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ITicketDocumentService
    {
        Task<IEnumerable<TicketDocument>> GetAllTicketDocumentsAsync();
        Task<TicketDocument> GetTicketDocumentByIdAsync(long Id);
        Task<TicketDocument> AddTicketDocumentAsync(TicketDocument Model);
    }
}
