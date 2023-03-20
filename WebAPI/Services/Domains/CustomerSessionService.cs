using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CustomerSessionService : ICustomerSessionService
    {
        private readonly ICustomerSessionRepo _repo;
        public CustomerSessionService(ICustomerSessionRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<CustomerSession>> GetAll()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<CustomerSession>> GetCustomerSessionByID(long Id)
        {
            return await _repo.GetAllAsync(x => x.ID == Id);
        }
        public async Task<IEnumerable<CustomerSession>> GetFireBasGetCustomerSessionFirebaseTokens(long Id, bool? isPushNotificationAllowed)
        {
            return await _repo.GetAllAsync(x => x.ID == Id);
        }


    }
}
