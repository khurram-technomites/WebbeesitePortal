using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICustomerSessionService
    {
        Task<IEnumerable<CustomerSession>> GetAll();
        Task<IEnumerable<CustomerSession>> GetCustomerSessionByID(long Id);

        Task<IEnumerable<CustomerSession>> GetFireBasGetCustomerSessionFirebaseTokens(long Id , bool? isPushNotificationAllowed);
    }
}
