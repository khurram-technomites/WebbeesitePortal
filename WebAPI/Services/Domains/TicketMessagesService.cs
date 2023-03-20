using HelperClasses.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class TicketMessagesService : ITicketMessageService
    {
        private readonly ITicketMessageRepo _repo;

        public TicketMessagesService(ITicketMessageRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<TicketMessages>> GetAllTicketMessagesAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id, ChildObjects: "TicketDocument");
        }
        public async Task<TicketMessages> GetTicketMessagesByIdAsync(long Id)
        {
            var ticketMessage = await _repo.GetAllAsync(x => x.Id == Id, ChildObjects: "TicketDocument , Ticket");
            return ticketMessage.FirstOrDefault();
        }
        public async Task<IEnumerable<TicketMessages>> GetTicketMessagesByTicketDocumentIdAsync(long Id)
        {
            var ticketMessage = await _repo.GetAllAsync(x => x.TicketDocumentId == Id, ChildObjects: "TicketDocument , Ticket");
            return ticketMessage;
        }
        public async Task<IEnumerable<TicketMessages>> GetTicketMessagesByTicketIdAsync(long Id)
        {
            var ticketMessage = await _repo.GetAllAsync(x => x.TicketId == Id, ChildObjects: "TicketDocument , Ticket");
            return ticketMessage;
        }
        public async Task<IEnumerable<TicketMessages>> GetTicketMessagesBySenderIdAsync(string Id)
        {
            var ticketMessage = await _repo.GetAllAsync(x => x.SenderId == Id, ChildObjects: "TicketDocument , Ticket");
            return ticketMessage;
        }
        public async Task<TicketMessages> AddTicketMessagestAsync(TicketMessages Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<TicketMessages> UpdateTicketMessages(TicketMessages Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
