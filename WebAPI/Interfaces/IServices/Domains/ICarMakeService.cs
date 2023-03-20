using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICarMakeService
    {
        Task<IEnumerable<CarMake>> GetAllCarMakesAsync();

        Task<IEnumerable<CarMake>> GetCarMakeByIdAsync(long Id);
        Task<IEnumerable<CarMake>> GetCarMakeByImageAsync(string Path);
        Task<CarMake> AddCarMakeAsync(CarMake Entity);
        Task<CarMake> UpdateCarMakeAsync(CarMake Entity);
        Task<CarMake> ArchiveCarMakeAsync(long Id);
    }
}
