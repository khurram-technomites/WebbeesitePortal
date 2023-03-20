using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    interface IOrderDetailsOptionValueService
    {
        Task<IEnumerable<OrderDetailOptionValue>> GetAllAsync(long OrderDetailId);
        Task<IEnumerable<OrderDetailOptionValue>> GetByIdAsync(long Id);
        Task<OrderDetailOptionValue> UpdateOrderDetailOptionValueAsync(OrderDetailOptionValue Model);
    }
}
