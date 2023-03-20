using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IClientEmailsService
    {
        Task<IEnumerable<ClientEmails>> GetAllClientEmailsAsync();
        Task<IEnumerable<ClientEmails>> GetClientEmailsByIdAsync(long Id);
        Task<IEnumerable<ClientEmails>> GetClientEmailsByClientIdAsync(long ClientId);
        Task<ClientEmails> AddClientEmailsAsync(ClientEmails Entity);
        Task<IEnumerable<ClientEmails>> AddClientEmailsRangeAsync(IEnumerable<ClientEmails> Entity);
        Task ArchiveClientEmailsAsync(long Id);
    }
}
