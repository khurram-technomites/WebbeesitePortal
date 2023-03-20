using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierOTPVerificationService
    {
        Task<IEnumerable<SupplierOTPVerification>> GetByEmailAsync(string Email);
        Task<IEnumerable<SupplierOTPVerification>> GetByPhoneAsync(string PhoneNumber);
        Task<SupplierOTPVerification> InsertAsync(SupplierOTPVerification Model);
        Task<SupplierOTPVerification> UpdateAsync(SupplierOTPVerification Model);
    }
}
