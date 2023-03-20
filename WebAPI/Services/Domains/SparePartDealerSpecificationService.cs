using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartDealerSpecificationService : ISparePartDealerSpecificationService
    {

        private readonly ISparePartDealerSpecificationRepo _repo;

        public SparePartDealerSpecificationService(ISparePartDealerSpecificationRepo repo)
        {
            _repo = repo;
        }
        public async Task DeleteSpecification(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<DealerInventorySpecification>> GetById(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<DealerInventorySpecification>> GetAllBySparePartsDealerInventorySpecificationAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<DealerInventorySpecification> AddSparePartsDealerInventorySpecificationAsync(DealerInventorySpecification Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task DeleteSparePartsDealerInventorySpecificationAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<DealerInventorySpecification>> GetAllBySparePartsAsync(long SparePartsDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartsDealerId == SparePartsDealerId);
        }

        public async Task<IEnumerable<DealerInventorySpecification>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<DealerInventorySpecification>> GetBySparePartsDealerId(long SparePartsDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartsDealerId == SparePartsDealerId);
        }
        public async Task<IEnumerable<DealerInventorySpecification>> GetBySparePartsCarMakeId(long carMakeId)
        {
            return await _repo.GetByIdAsync(x => x.CarMakeId == carMakeId);
        }
        public async Task<IEnumerable<DealerInventorySpecification>> GetBySparePartsCarModelId(long carModelId)
        {
            return await _repo.GetByIdAsync(x => x.CarModelId == carModelId);
        }

        public async Task<DealerInventorySpecification> UpdateSparePartsDealerInventorySpecificationAsync(DealerInventorySpecification Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
