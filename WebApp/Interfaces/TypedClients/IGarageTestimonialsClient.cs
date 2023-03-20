using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageTestimonialsClient
    {
        Task<IEnumerable<GarageTestimonialsDTO>> GetAllAsync();
        Task<IEnumerable<GarageTestimonialsDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageTestimonialsDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<long> GetAllCountByGarageIdAsync(long GarageId);
        Task<GarageTestimonialsDTO> AddGarageTestimonialsAsync(GarageTestimonialsDTO Entity);
        Task<GarageTestimonialsDTO> UpdateGarageTestimonialsAsync(GarageTestimonialsDTO Entity);
        Task DeleteGarageTestimonialsAsync(long Id);
        Task<GarageTestimonialsDTO> ToggleActiveStatus(long Id);

    }
}
