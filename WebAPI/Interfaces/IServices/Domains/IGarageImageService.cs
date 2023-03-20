using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageImageService
    {
        Task<IEnumerable<GarageImage>> GetGarageImagesByImagePath(string Path);
        Task<IEnumerable<GarageImage>> GetImagesByGarage(long GarageId);
        Task DeleteGarageImage(long Id);
    }
}
