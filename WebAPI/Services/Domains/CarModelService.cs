using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CarModelService : ICarModelService
    {
        private readonly ICarModelRepo _repo;
        public CarModelService(ICarModelRepo repo)
        {
            _repo = repo;
        }
        public async Task<CarModel> AddCarModelAsync(CarModel Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<CarModel> ArchiveCarModelAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<CarModel>> GetAllAsync()
        {
            return await _repo.GetAllAsync(ChildObjects: "CarMake");
        }

        public async Task<IEnumerable<CarModel>> GetByCarMakeAsync(long MakeId)
        {
            return await _repo.GetByIdAsync(x => x.CarMakeId == MakeId);
        }

        public async Task<IEnumerable<CarModel>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id , ChildObjects: "CarMake" );
        }

        public async Task<CarModel> UpdateCarModelAsync(CarModel Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
