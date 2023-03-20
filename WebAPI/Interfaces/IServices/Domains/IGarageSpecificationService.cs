
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageSpecificationService
    {
        Task<IEnumerable<GarageRepairSpecification>> GetById(long Id);
        Task<IEnumerable<GarageRepairSpecification>> GetByIdandCarMake(long Id, long CarMakeId);
        Task<GarageRepairSpecification> Add(GarageRepairSpecification Model);
        Task DeleteRepairSpecification(long Id);
    }
}
