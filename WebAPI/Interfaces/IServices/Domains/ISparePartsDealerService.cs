using HelperClasses.DTOs;
using HelperClasses.DTOs.SparePartsDealer.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartsDealerService
    {
        Task<IEnumerable<SparePartsDealer>> GetAllSparePartsDealerAsync(SparePartFilterDTO Filter);
        Task<long> GetAllSparePartsDealersCountAsync();
        Task<IEnumerable<SparePartsDealer>> GetAllSparePartsDealerAsync();
        Task<IEnumerable<SparePartsDealer>> GetDealerByOrigin(string Origin, string Section = "Default");
        Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerByIdAsync(long Id);
        Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerByLogoAsync(string Path);
        Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerByVideoAsync(string Path);
        Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerBySlugAsync(string Slug);
        Task<IEnumerable<SparePartsDealer>> GetSparePartsDealerByUserAsync(string UserId);
        Task<SparePartsDealer> AddSparePartsDealerAsync(SparePartsDealer Entity);
        Task<SparePartsDealer> UpdateSparePartsDealerAsync(SparePartsDealer Entity);
        Task<SparePartsDealer> ArchiveSparePartsDealerAsync(long Id);
        Task<double> ActiveIactiveCount();
    }
}
