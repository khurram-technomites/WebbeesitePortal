using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartDealerSpecificationService
    {
        Task<IEnumerable<DealerInventorySpecification>> GetById(long Id);
        Task DeleteSpecification(long Id);

        Task<IEnumerable<DealerInventorySpecification>> GetAllBySparePartsDealerInventorySpecificationAsync();

        Task<IEnumerable<DealerInventorySpecification>> GetBySparePartsDealerId(long SparePartsDealerId);
        Task<IEnumerable<DealerInventorySpecification>> GetBySparePartsCarMakeId(long CarMakeId);
        Task<IEnumerable<DealerInventorySpecification>> GetBySparePartsCarModelId(long CarModelId);
        Task<DealerInventorySpecification> AddSparePartsDealerInventorySpecificationAsync(DealerInventorySpecification Model);
        Task<DealerInventorySpecification> UpdateSparePartsDealerInventorySpecificationAsync(DealerInventorySpecification Model);
        Task DeleteSparePartsDealerInventorySpecificationAsync(long Id);
    }
}
