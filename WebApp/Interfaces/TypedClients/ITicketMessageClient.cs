using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface ITicketMessageClient
    {
        Task<IEnumerable<TicketMessagesDTO>> GetAllTicketsAsync();
        Task<TicketMessagesDTO> GetTicketMessageById(long Id);
        Task<IEnumerable<TicketMessagesDTO>> GetTicketMessageByTicketId(long Id);
        Task<TicketMessagesDTO> CreateTicketMessage(TicketMessageViewModel model);
    }
}
