using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartsDealerInventorySpecificationClient
    {
        Task<IEnumerable<SparePartsDealerSpecificationsDTO>> GetSparePartsDealerInventorySpecificationBySpareParts();
        Task<SparePartsDealerSpecificationsDTO> GetSparePartsDealerInventorySpecificationByID(long Id);
        Task<SparePartsDealerSpecificationsDTO> GetSparePartsDealerInventorySpecificationByCarMakeID(long CarMakeId);
        Task<SparePartsDealerSpecificationsDTO> GetSparePartsDealerInventorySpecificationByCarModelID(long CarMakeId);
        Task<SparePartsDealerSpecificationsDTO> GetSparePartsDealerInventorySpecificationBySparePartsDealerID(long CarMakeId);
        Task<SparePartsDealerSpecificationsDTO> AddSparePartsDealerInventorySpecification(SparePartsDealerSpecificationsDTO model);
        Task<SparePartsDealerSpecificationsDTO> UpdateSparePartsDealerInventorySpecification(SparePartsDealerSpecificationsDTO model);
        Task DeleteSparePartsDealerInventorySpecification(long Id);
    }
}
