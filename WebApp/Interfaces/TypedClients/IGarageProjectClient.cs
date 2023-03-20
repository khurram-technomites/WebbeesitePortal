using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageProjectClient
    {
        Task<GarageProjectDTO> GetByIdAsync(long Id);
        Task<IEnumerable<GarageProjectDTO>> GetByGarageAsync(long GarageId);
        Task<long> GetCountByGarageAsync(long GarageId);
        Task<GarageProjectDTO> AddProjectAsync(GarageProjectDTO Model);
        Task<GarageProjectDTO> UpdateProjectAsync(GarageProjectDTO Model);
        Task<GarageProjectDTO> ArchiveProjectAsync(long Id);
        Task<GarageProjectDTO> ToggleStatusAsync(long Id);
    }
}
