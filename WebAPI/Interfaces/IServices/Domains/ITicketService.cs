using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<IEnumerable<Ticket>> GetTicketsByOpenStatus();
        Task<IEnumerable<Ticket>> GetTicketsByModule(string Module);
        Task<Ticket> GetTicket(long Id);
        Task<long> GetAllTicketsCountAsync();
        Task<IEnumerable<Ticket>> GetTicketsByRestaurant(long restaurantId);
        Task<IEnumerable<Ticket>> GetTicketsBySupplier(long supplierId);
        Task<Ticket> GetTicketsByUser(string userID);
        Task<Ticket> Create(Ticket Model);
        Task<Ticket> Update(Ticket Model);
        //bool CreateTicket(Ticket ticket, ref string message);
        //bool UpdateTicket(ref Ticket ticket, ref string message);
        //bool DeleteTicket(long Id);
    }
}
