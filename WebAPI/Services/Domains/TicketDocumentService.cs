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
    public class TicketDocumentService : ITicketDocumentService 
    {
        private readonly ITicketDecoumetnRepo _repo;

        public TicketDocumentService(ITicketDecoumetnRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<TicketDocument>> GetAllTicketDocumentsAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id);
        }
        public async Task<TicketDocument> GetTicketDocumentByIdAsync(long Id)
        {
            var ticketDocuments = await _repo.GetAllAsync(x => x.Id == Id);
            return ticketDocuments.FirstOrDefault();
        }
        public async Task<TicketDocument> AddTicketDocumentAsync(TicketDocument Model)
        {
            return await _repo.InsertAsync(Model);
        }
    }
}
