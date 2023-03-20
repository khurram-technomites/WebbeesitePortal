using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICarModelClient
    {
        Task<IEnumerable<CarModelDTO>> GetAllCarModelsAsync();
        Task<CarModelDTO> GetCarModelByIdAsync(long CarModelId);
        Task<CarModelDTO> GetCarModelByMakeIdAsync(long CarMakeId);

        Task<CarModelDTO> AddCarModelAsync(CarModelDTO Entity);
        Task<CarModelDTO> UpdateCarModelAsync(CarModelDTO Entity);
        Task DeleteCarModelAsync(long CarModelId);
        Task<CarModelDTO> ToggleActiveStatus(long CarModelId);
    }
}
