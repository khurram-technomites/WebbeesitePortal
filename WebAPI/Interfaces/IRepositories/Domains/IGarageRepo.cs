using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface IGarageRepo : IRepository<Garage>
    {
        IEnumerable<GarageCardResponseDTO> GetAllNearMe(GarageFilter Filter);
    }
}
