using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ITicketMessageService
    {
        Task<IEnumerable<TicketMessages>> GetAllTicketMessagesAsync();
        Task<TicketMessages> GetTicketMessagesByIdAsync(long Id);
        Task<IEnumerable<TicketMessages>> GetTicketMessagesByTicketDocumentIdAsync(long Id);
        Task<IEnumerable<TicketMessages>> GetTicketMessagesBySenderIdAsync(string Id);
        Task<IEnumerable<TicketMessages>> GetTicketMessagesByTicketIdAsync(long Id);
        Task<TicketMessages> AddTicketMessagestAsync(TicketMessages Entity);
        Task<TicketMessages> UpdateTicketMessages(TicketMessages Model);
    }
}
