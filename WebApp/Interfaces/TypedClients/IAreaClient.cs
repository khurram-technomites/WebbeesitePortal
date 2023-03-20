using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IAreaClient
    {
        Task<IEnumerable<AreaDTO>> GetAreas();
        Task<AreaDTO> GetAreaByID(long Id);
        Task<AreaDTO> Create(AreaDTO model);
        Task<AreaDTO> Edit(AreaDTO model);

        Task<AreaDTO> Delete(long Id);
    }
}
