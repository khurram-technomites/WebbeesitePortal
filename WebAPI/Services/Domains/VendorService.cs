using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepo _repo;
        public VendorService(IVendorRepo repo)
        {
            _repo = repo;
        }
        public async Task<Vendor> AddVendorAsync(Vendor Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<Vendor>> GetAllAsync()
        {
            return await _repo.GetAllAsync(x => x.ArchivedDate == null); ;
        }

        public async Task<IEnumerable<Vendor>> GetVendorByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "User");
        }

        public async Task<Vendor> UpdateVendorAsync(Vendor Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<IEnumerable<Vendor>> GetVendorByUserAsync(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.UserId == UserId, ChildObjects: "User");
        }

        public async Task<Vendor> ArchiveVendorAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
