using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebApp.Interfaces.TypedClients
{
    public interface IClientDomainSuggestionsClient
    {
        Task<IEnumerable<ClientDomainSuggestionsDTO>> GetClientContentDomain();
        Task<IEnumerable<ClientDomainSuggestionsDTO>> GetClientContentDomainByClientId(long ClientId);
        Task<ClientDomainSuggestionsDTO> GetClientContentDomainByID(long Id);
        Task<ClientDomainSuggestionsDTO> Create(ClientDomainSuggestionsDTO model);
        Task<IEnumerable<ClientDomainSuggestionsDTO>> CreateRange(IEnumerable<ClientDomainSuggestionsDTO> model);
        Task Delete(long Id);
    }
}
