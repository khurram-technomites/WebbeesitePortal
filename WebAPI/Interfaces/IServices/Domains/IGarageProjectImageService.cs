using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageProjectImageService
    {
        Task<IEnumerable<GarageProjectImages>> GetByProjectId(long ProjectId);
        Task<IEnumerable<GarageProjectImages>> GetById(long Id);
        Task<IEnumerable<GarageProjectImages>> GetByPath(string Path);
        Task<GarageProjectImages> AddImage(GarageProjectImages Model);
        Task DeleteImage(long Id);

    }
}
