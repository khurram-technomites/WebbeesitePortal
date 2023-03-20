using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IDealerImageService
    {
        Task<IEnumerable<DealerImage>> GetAllDealerImagesAsync(PagingParameters Pagination);
        Task<IEnumerable<DealerImage>> GetDealerImageByIdAsync(long Id);
        Task<IEnumerable<DealerImage>> GetDealerImageBySparePartsDealerIdAsync(long sparePartsDealerId);
        Task<IEnumerable<DealerImage>> GetDealerImageByImagePathAsync(string Path);
        Task<DealerImage> AddDealerImageAsync(DealerImage Entity);
        Task<DealerImage> UpdateDealerImageAsync(DealerImage Entity);
        Task<DealerImage> ArchiveDealerImageAsync(long Id);
        Task DeleteDealerImageAsync(long Id);
    }
}
