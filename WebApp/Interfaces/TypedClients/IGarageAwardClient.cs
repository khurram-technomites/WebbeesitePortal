using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageAwardClient
    {
        Task<IEnumerable<GarageAwardDTO>> GetAllGarageAwardsAsync(long GarageId);
        Task<GarageAwardDTO> GetGarageAwardByIdAsync(long GarageAwardId);
        Task<GarageAwardDTO> AddGarageAwardAsync(GarageAwardDTO Entity);
        Task<long> GetCountAllGarageAwardsAsync(long GarageId);
        Task<GarageAwardDTO> UpdateGarageAwardAsync(GarageAwardDTO Entity);
        Task DeleteGarageAwardAsync(long GarageAwardId);
        
    }
}
