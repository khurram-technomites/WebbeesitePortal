using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class ModulePurchaseDetailsService:IModulePurchaseDetailsService
    {
        private readonly IModulePurchaseDetailsRepo _repo;
        public ModulePurchaseDetailsService(IModulePurchaseDetailsRepo repo)
        {
            _repo = repo;
        }

        public async Task<ModulePurchaseDetails> AddAsync(ModulePurchaseDetails Entity)
        {
            return await _repo.InsertAsync(Entity);
        }
        public async Task<IEnumerable<ModulePurchaseDetails>> AddRangeAsync(List<ModulePurchaseDetails> Entity)
        {
            return await _repo.InsertRangeAsync(Entity);
        }
        public async Task<IEnumerable<ModulePurchaseDetails>> UpdateRangeAsync(List<ModulePurchaseDetails> Entity)
        {
            return await _repo.UpdateRangeAsync(Entity);
        }

        public async Task<IEnumerable<ModulePurchaseDetails>> GetDetailsByPurchaseId(long PurchaseId)
        {
            return await _repo.GetAllAsync(x => x.ClientModulePurchaseID == PurchaseId && x.ArchivedDate == null, ChildObjects: "Module");
        }
        public async Task<IEnumerable<ModulePurchaseDetails>> GetDetailsByPurchaseIdandName(long PurchaseId,string Name)
        {
            return await _repo.GetAllAsync(x => x.ClientModulePurchaseID == PurchaseId && x.Module.ServiceName == Name);
        }


        public async Task<IEnumerable<ModulePurchaseDetails>> GetDetailsByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "ClientModulePurchases");
        }


        public async Task<ModulePurchaseDetails> UpdateAsync(ModulePurchaseDetails Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
        public async Task DeleteAsync(long Id)
        {
             await _repo.DeleteAsync(Id);
        }


    }
}
