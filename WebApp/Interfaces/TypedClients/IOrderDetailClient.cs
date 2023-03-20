using HelperClasses.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IOrderDetailClient
    {
        Task<IEnumerable<OrderDetailDTO>> GetAllOrderDetailsAsync(long OrderId);
    }
}
