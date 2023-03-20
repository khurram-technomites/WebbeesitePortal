using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageSubscribersService
    {
        Task<IEnumerable<GarageSubscribers>> GetAllAsync();
        Task<IEnumerable<GarageSubscribers>> GetGarageSubscribersByIdAsync(long Id);
        Task<IEnumerable<GarageSubscribers>> GetGarageSubscribersByEmailAsync(string Email, long GarageId);
        Task<IEnumerable<GarageSubscribers>> GetGarageSubscribersByGarageIdAsync(long GaragedId);
        Task<GarageSubscribers> AddGarageSubscribersAsync(GarageSubscribers Model);
        Task<GarageSubscribers> UpdateGarageSubscribersAsync(GarageSubscribers Model);
        Task ArchiveGarageSubscribersAsync(long Id);
    }
}
