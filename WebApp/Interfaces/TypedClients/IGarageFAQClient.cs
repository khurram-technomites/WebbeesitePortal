using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageFAQClient
    {
        Task<GarageFAQDTO> AddFAQAsync(GarageFAQDTO Model);
        Task<GarageFAQDTO> UpdateFAQAsync(GarageFAQDTO Model);
        Task<GarageFAQDTO> SavePosition(GarageFAQDTO Model);
        Task<GarageFAQDTO> ArchiveFAQAsync(long Id);
        Task<IEnumerable<GarageFAQDTO>> GetFAQByGarageAsync(long GarageId);
        Task<GarageFAQDTO> GetFAQByIdAsync(long Id);
    }
}
