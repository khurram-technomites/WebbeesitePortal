using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageCustomerClient
    {
        Task<object> Paid(long orderId, string paymentId);
    }
}
