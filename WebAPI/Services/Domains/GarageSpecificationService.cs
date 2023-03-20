using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageSpecificationService : IGarageSpecificationService
    {
        private readonly IGarageSpecificationRepo _repo;

        public GarageSpecificationService(IGarageSpecificationRepo repo)
        {
            _repo = repo;
        }

        public async Task<GarageRepairSpecification> Add(GarageRepairSpecification Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task DeleteRepairSpecification(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<GarageRepairSpecification>> GetById(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id , ChildObjects : "CarMake");
        }

        public async Task<IEnumerable<GarageRepairSpecification>> GetByIdandCarMake(long Id, long CarMakeId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == Id && x.CarMakeId == CarMakeId, ChildObjects: "CarMake");
        }
    }
}
