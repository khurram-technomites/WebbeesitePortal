using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class ClientModulePurchasesService:IClientModulePurchasesService
    {
        private readonly IClientModulePurchasesRepo _repo;
        public ClientModulePurchasesService(IClientModulePurchasesRepo repo)
        {
            _repo = repo;
        }

        public async Task<ClientModulePurchases> AddAsync(ClientModulePurchases Entity)
        {
            return await _repo.InsertAsync(Entity);
        }


        public async Task<IEnumerable<ClientModulePurchases>> GetPurchaseByGarageId(long GarageID)
        {
            return await _repo.GetAllAsync(x => x.GarageID == GarageID && x.ArchivedDate == null);
        }


        public async Task<IEnumerable<ClientModulePurchases>> GetPurchaseByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Garage");
        }


        public async Task<ClientModulePurchases> UpdateAsync(ClientModulePurchases Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
        public async Task<IEnumerable<ClientModulePurchases>> GetEarning(long VendorId)
        {
           return await _repo.GetAllAsync(x => x.Garage.VendorId == VendorId && x.PaymentStatus == Enum.GetName(typeof(ClientPaymentStatus), ClientPaymentStatus.Paid));
           
        }
    }
}
