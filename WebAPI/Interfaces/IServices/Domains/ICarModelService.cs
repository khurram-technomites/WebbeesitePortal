using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICarModelService
    {
        Task<IEnumerable<CarModel>> GetAllAsync();
        Task<IEnumerable<CarModel>> GetByIdAsync(long Id);
        Task<IEnumerable<CarModel>> GetByCarMakeAsync(long MakeId);
        Task<CarModel> AddCarModelAsync(CarModel Model);
        Task<CarModel> UpdateCarModelAsync(CarModel Model);
        Task<CarModel> ArchiveCarModelAsync(long Id);
    }
}
