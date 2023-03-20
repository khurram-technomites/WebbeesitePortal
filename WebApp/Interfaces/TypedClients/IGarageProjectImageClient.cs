using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageProjectImageClient
    {
        Task<IEnumerable<GarageProjectImageDTO>> GetByProject(long ProjectId);
        Task<GarageProjectImageDTO> AddProjectImage(GarageProjectImageDTO Model);
    }
}
