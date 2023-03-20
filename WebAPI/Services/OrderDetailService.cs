using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class OrderDetailService  : IOrderDetailService
    {
        private readonly IOrderDetailRepo _repo;

        public OrderDetailService(IOrderDetailRepo repo)
        {
            _repo = repo;
        }


        public async Task<IEnumerable<OrderDetail>> GetAllAsync(long OrderId)
        {
            return await _repo.GetAllAsync(x => x.OrderId == OrderId, ChildObjects: "OrderDetailOptionValues");
        }


        public async Task<IEnumerable<OrderDetail>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "OrderDetailOptionValues");
        }

        public async Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
