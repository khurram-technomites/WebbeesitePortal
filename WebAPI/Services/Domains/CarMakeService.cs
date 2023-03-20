using HelperClasses.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CarMakeService : ICarMakeService
    {
        private readonly ICarMakeRepo _repo;
        public CarMakeService(ICarMakeRepo repo)
        {
            _repo = repo;
        }

        public async Task<CarMake> AddCarMakeAsync(CarMake Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<CarMake> ArchiveCarMakeAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<CarMake>> GetAllCarMakesAsync()
        {
            return await _repo.GetAllAsync(ChildObjects: "CarModels");
        }

        public async Task<IEnumerable<CarMake>> GetCarMakeByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<CarMake>> GetCarMakeByImageAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Logo == Path);
        }

        public async Task<CarMake> UpdateCarMakeAsync(CarMake Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
