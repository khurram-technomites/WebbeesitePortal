using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class OrderDetailOptionValueService : IOrderDetailsOptionValueService
    {
        private readonly IOrderDetailsOptionValueRepo _repo;

        public OrderDetailOptionValueService(IOrderDetailsOptionValueRepo repo)
        {
            _repo = repo;
        }


        public async Task<IEnumerable<OrderDetailOptionValue>> GetAllAsync(long OrderDetailId)
        {
            return await _repo.GetAllAsync(x => x.OrderDetailId == OrderDetailId);
        }


        public async Task<IEnumerable<OrderDetailOptionValue>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<OrderDetailOptionValue> UpdateOrderDetailOptionValueAsync(OrderDetailOptionValue Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task OrderDetailOptionValueDelete(long Id)
        {
            await _repo.DeleteAsync(Id);
        }
    }
}
