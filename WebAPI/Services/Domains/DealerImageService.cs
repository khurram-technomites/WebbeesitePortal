using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class DealerImageService : IDealerImageService
    {
        private readonly IDealerImageRepo _repo;
        public DealerImageService(IDealerImageRepo repo)
        {
            _repo = repo;
        }

        public async Task<DealerImage> AddDealerImageAsync(DealerImage Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<DealerImage> ArchiveDealerImageAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task DeleteDealerImageAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<DealerImage>> GetAllDealerImagesAsync(PagingParameters Pagination)
        {
            return await _repo.GetAllAsync(Pagination: Pagination, OrderExp: x => x.Id, ChildObjects: "User");
        }

        public async Task<IEnumerable<DealerImage>> GetDealerImageByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<DealerImage>> GetDealerImageBySparePartsDealerIdAsync(long SparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == SparePartDealerId);
        }
        public async Task<IEnumerable<DealerImage>> GetDealerImageByImagePathAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Image == Path);
        }

        public async Task<DealerImage> UpdateDealerImageAsync(DealerImage Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
