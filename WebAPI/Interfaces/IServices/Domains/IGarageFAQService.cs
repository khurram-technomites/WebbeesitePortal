using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageFAQService
    {
        Task<GarageFAQ> AddFAQAsync(GarageFAQ Model);
        Task<GarageFAQ> UpdateFAQAsync(GarageFAQ Model);
        Task<long> MaxCount(long GarageId);
        Task<GarageFAQ> ArchiveFAQAsync(long Id);
        Task<IEnumerable<GarageFAQ>> GetFAQByGarageAsync(long GarageId);
        Task<IEnumerable<GarageFAQ>> GetFAQByIdAsync(long Id);

    }
}
