using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface ITicketClient
    {
        Task<IEnumerable<TicketDTO>> GetAllTicketsAsync();
        Task<IEnumerable<TicketDTO>> GetAllTicketsByModuleAsync(string Module);
        Task<IEnumerable<TicketDTO>> GetTicketsByOpenStatus();
        Task<TicketDTO> GetTicket(long Id);
        Task<IEnumerable<TicketDTO>> GetTicketsByRestaurant(long restaurantId);
        Task<IEnumerable<TicketDTO>> GetTicketsBySupplier(long supplierId);
        Task<IEnumerable<TicketDTO>> GetTicketsByUser(string userID);
        Task<TicketDTO> UpdateTicket(TicketViewModel model);
        Task<TicketDTO> UpdateStatus(TicketViewModel model);
        Task<TicketDTO> CreateTicket(TicketViewModel model);
    }
}
