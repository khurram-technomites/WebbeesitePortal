using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class VendorOTPVerificationService:IVendorOTPVerificationService
    {
        private readonly IVendorOTPVerificationRepo _repo;

        public VendorOTPVerificationService(IVendorOTPVerificationRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<VendorOTPVerification>> GetByEmailAsync(string Email)
        {
            return await _repo.GetByIdAsync(x => x.Email == Email);
        }

        public async Task<IEnumerable<VendorOTPVerification>> GetByPhoneAsync(string PhoneNumber)
        {
            return await _repo.GetByIdAsync(x => x.PhoneNumber == PhoneNumber);
        }

        public async Task<VendorOTPVerification> InsertAsync(VendorOTPVerification Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<VendorOTPVerification> UpdateAsync(VendorOTPVerification Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
