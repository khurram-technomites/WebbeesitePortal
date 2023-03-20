using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync(long restaurantId);
        Task<IEnumerable<OrderDetail>> GetByIdAsync(long Id);
        Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail Model);
    }
}
