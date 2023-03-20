using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IVendorOTPVerificationService
    {
        Task<IEnumerable<VendorOTPVerification>> GetByEmailAsync(string Email);
        Task<IEnumerable<VendorOTPVerification>> GetByPhoneAsync(string PhoneNumber);
        Task<VendorOTPVerification> InsertAsync(VendorOTPVerification Model);
        Task<VendorOTPVerification> UpdateAsync(VendorOTPVerification Model);
    }
}
