using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IVendorService
    {
        Task<IEnumerable<Vendor>> GetAllAsync();
        Task<IEnumerable<Vendor>> GetVendorByIdAsync(long Id);
        Task<Vendor> AddVendorAsync(Vendor Model);
        Task<Vendor> UpdateVendorAsync(Vendor Model);
        Task<IEnumerable<Vendor>> GetVendorByUserAsync(string UserId);

        Task<Vendor> ArchiveVendorAsync(long Id);
    }
}
