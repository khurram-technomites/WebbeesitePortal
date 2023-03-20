
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IClientDomainSuggestionsService
    {
        Task<IEnumerable<ClientDomainSuggestions>> GetAllClientDomainAsync();
        Task<IEnumerable<ClientDomainSuggestions>> GetClientDomainByIdAsync(long Id);
        Task<IEnumerable<ClientDomainSuggestions>> GetClientDomainByClientIdAsync(long ClientId);
        Task<ClientDomainSuggestions> AddClientDomainAsync(ClientDomainSuggestions Entity);
        Task<IEnumerable<ClientDomainSuggestions>> AddClientDomainRangeAsync(IEnumerable<ClientDomainSuggestions> Entity);
        Task ArchiveClientDomainAsync(long Id);
    }
}
