using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISubscriberClient
    {
        Task<IEnumerable<SubscriberDTO>> GetSubscribers(PagingParameters Model);
        Task<SubscriberDTO> GetEmailByID(long Id);
        Task<SubscriberDTO> Create(SubscriberDTO model);
        Task<SubscriberDTO> Edit(SubscriberDTO model);

        Task<SubscriberDTO> Delete(long Id);

        Task<IEnumerable<SubscriberDTO>> GetsubscribersDateWise(DateTime FromDate, DateTime ToDate);
        Task<SubscriberDTO> SendMessage(string Email , string Subject , string Message);
    }
}
