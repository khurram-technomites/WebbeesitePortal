using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierOTPVerificationService : ISupplierOTPVerificationService
    {
        private readonly ISupplierOTPVerificationRepo _repo;

        public SupplierOTPVerificationService(ISupplierOTPVerificationRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SupplierOTPVerification>> GetByEmailAsync(string Email)
        {
            return await _repo.GetByIdAsync(x => x.Email == Email);
        }

        public async Task<IEnumerable<SupplierOTPVerification>> GetByPhoneAsync(string PhoneNumber)
        {
            return await _repo.GetByIdAsync(x => x.PhoneNumber == PhoneNumber);
        }

        public async Task<SupplierOTPVerification> InsertAsync(SupplierOTPVerification Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SupplierOTPVerification> UpdateAsync(SupplierOTPVerification Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
