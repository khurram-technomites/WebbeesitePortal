using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageProjectService
    {
        Task<IEnumerable<GarageProject>> GetByIdAsync(long Id);
        Task<long> GetAllProjectsByGarageIdAsync(long GarageId);
        Task<IEnumerable<GarageProject>> GetByGarageAsync(long GarageId);
        Task<long> GetCountByGarageAsync(long GarageId);
        Task<GarageProject> AddProjectAsync(GarageProject Model);
        Task<GarageProject> UpdateProjectAsync(GarageProject Model);
        Task<GarageProject> ArchiveProjectAsync(long Id);
    }
}
