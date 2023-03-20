using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SubscriberService : ISubscribeService
    {
        private readonly ISubscriberRepo _repo;
        public SubscriberService(ISubscriberRepo repo)
        {
            _repo = repo;
        }

        public async Task<Subscriber> AddSubscriberAsync(Subscriber Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<Subscriber> ArchiveSubscriberAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<IEnumerable<Subscriber>> GetFilteredSubscribers(DateTime FromDate, DateTime ToDate)
        {
            return await _repo.GetAllAsync(OrderExp : c => c.CreationDate >= FromDate && c.CreationDate <= ToDate);
        }

        public async Task<IEnumerable<Subscriber>> GetAllSubscribersAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id);
        }

        public async Task<IEnumerable<Subscriber>> GetSubscriberByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<Subscriber> UpdateSubscriberAsync(Subscriber Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
