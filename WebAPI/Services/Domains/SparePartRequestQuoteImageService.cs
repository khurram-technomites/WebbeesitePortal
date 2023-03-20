using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartRequestQuoteImageService: ISparePartRequestQuoteImageService
    {
        private readonly ISparePartRequestQuoteImageRepo _repo;

        public SparePartRequestQuoteImageService(ISparePartRequestQuoteImageRepo repo)
        {
            _repo = repo;
        }
        public async Task DeleteAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<SparePartRequestQuoteImage>> GetRequestByImageAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Image == Path);
        }
        public async Task<IEnumerable<SparePartRequestQuoteImage>> GetBySparePartRequestQuoteIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.SparePartRequestQuoteId == Id);
        }

    }
}
