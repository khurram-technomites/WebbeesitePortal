using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class VendorDocumentService : IVendorDocumentService
    {
        private readonly IVendorDocumentRepo _repo;

        public VendorDocumentService(IVendorDocumentRepo repo)
        {
            _repo = repo;
        }

        public async Task<VendorDocument> AddDocument(VendorDocument Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task DeleteRecord(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<VendorDocument> EditDocument(VendorDocument Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<IEnumerable<VendorDocument>> GetByVendor(long VendorId)
        {
            return await _repo.GetByIdAsync(x => x.VendorId == VendorId);
        }

        public async Task<IEnumerable<VendorDocument>> GetByID(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

      
    }
}
