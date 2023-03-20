using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageAwardService
    {
        Task<IEnumerable<GarageAward>> GetAllGarageAwardsAsync(long GarageId);
        Task<long> GetAllAwardByGarageIdAsync(long GarageId);
        Task<long> GetCountAllGarageAwardsAsync(long GarageId);
        Task<IEnumerable<GarageAward>> GetGarageAwardByIdAsync(long Id);
        Task<GarageAward> AddGarageAwardAsync(GarageAward Entity);
        Task<GarageAward> UpdateGarageAwardAsync(GarageAward Entity);
        Task<GarageAward> ArchiveGarageAwardAsync(long Id);
    }
}
