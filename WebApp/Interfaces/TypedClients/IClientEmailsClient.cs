using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebApp.Interfaces.TypedClients
{
    public interface IClientEmailsClient
    {
        Task<IEnumerable<ClientEmailsDTO>> GetClientEmails();
        Task<IEnumerable<ClientEmailsDTO>> GetClientEmailsByClientId(long ClientId);
        Task<ClientEmailsDTO> GetClientEmailsByID(long Id);
        Task<ClientEmailsDTO> Create(ClientEmailsDTO model);
        Task<IEnumerable<ClientEmailsDTO>> CreateRange(IEnumerable<ClientEmailsDTO> model);

        Task Delete(long Id);
    }
}
