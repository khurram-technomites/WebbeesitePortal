using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISubscribeService
    {
        Task<IEnumerable<Subscriber>> GetAllSubscribersAsync();
        Task<IEnumerable<Subscriber>> GetSubscriberByIdAsync(long Id);
        Task<Subscriber> AddSubscriberAsync(Subscriber Entity);
        Task<Subscriber> UpdateSubscriberAsync(Subscriber Entity);
        Task<Subscriber> ArchiveSubscriberAsync(long Id);
        Task<IEnumerable<Subscriber>> GetFilteredSubscribers(DateTime FromDate, DateTime ToDate);
    }
}
