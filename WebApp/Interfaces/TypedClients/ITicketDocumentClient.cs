using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface ITicketDocumentClient
    {
        Task<IEnumerable<TicketDocumentDTO>> GetAllDocumetsAsync();
        Task<TicketDocumentDTO> GetTicketDocumentById(long Id);
        Task<TicketDocumentDTO> CreateTicketDocument(TicketDocumentViewModel model);
    }
}
