using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageRepairClient
    {
        Task<GarageRepairSpecificationViewModel> Add(long GarageId, long CarMakeId);
        Task Delete(long GarageId, long CarMakeId);
    }
}
