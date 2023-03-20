using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageExpertiseService
    {
        Task<IEnumerable<GarageExpertise>> GetAllAsync();
        Task<IEnumerable<GarageExpertise>> GetGarageExpertiseByIdAsync(long Id);
        Task<IEnumerable<GarageExpertise>> GetGarageExpertiseByGarageExpertiseManagementIdAsync(long GarageExpertiseManagementId);
        Task<GarageExpertise> AddGarageExpertiseAsync(GarageExpertise Model);
        Task<GarageExpertise> UpdateGarageExpertiseAsync(GarageExpertise Model);
        Task<GarageExpertise> ArchiveGarageExpertiseAsync(long Id);
        Task DeleteGarageExpertiseAsync(long Id);
    }
}
