using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartSubscriberService
    {
        Task<IEnumerable<SparePartSubscriber>> GetAllAsync();
        Task<IEnumerable<SparePartSubscriber>> GetSparePartSubscriberByIdAsync(long Id);
        Task<IEnumerable<SparePartSubscriber>> GetSparePartSubscribersByEmailAsync(long Id, string Email);
        Task<IEnumerable<SparePartSubscriber>> GetSparePartSubscriberBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartSubscriber> AddSparePartSubscriberAsync(SparePartSubscriber Model);
        Task<SparePartSubscriber> UpdateSparePartSubscriberAsync(SparePartSubscriber Model);
        Task ArchiveSparePartSubscriberAsync(long Id);
    }
}
