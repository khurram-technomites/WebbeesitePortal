using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebApp.Interfaces.TypedClients
{
    public interface  IClientContentMediaClient
    {
        Task<IEnumerable<ClientContentMediaDTO>> GetClientContent();
        Task<IEnumerable<ClientContentMediaDTO>> GetClientContentByClientId(long ClientId);
        Task<ClientContentMediaDTO> GetClientContentByID(long Id);
        Task<ClientContentMediaDTO> Create(ClientContentMediaDTO model);
        Task<IEnumerable<ClientContentMediaDTO>> CreateRange(IEnumerable<ClientContentMediaDTO> model);

        Task Delete(long Id);
    }
}
