using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IClientContentMediaService
    {
        Task<IEnumerable<ClientContentMedia>> GetAllClientContentMediaAsync();
        Task<IEnumerable<ClientContentMedia>> GetClientContentMediaByIdAsync(long Id);
        Task<IEnumerable<ClientContentMedia>> GetClientContentMediaByClientIdAsync(long ClienId);
        Task<ClientContentMedia> AddClientContentMediaAsync(ClientContentMedia Entity);
        Task<IEnumerable<ClientContentMedia>> AddClientContentMediaRangeAsync(IEnumerable<ClientContentMedia> Entity);
        Task ArchiveClientContentMediaAsync(long Id);
    }
}
