using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICarMakeClient
    {
        Task<IEnumerable<CarMakeDTO>> GetAllCarMakesAsync();
        Task<CarMakeDTO> GetCarMakeByIdAsync(long CarMakeId);
        Task<CarMakeDTO> AddCarMakeAsync(CarMakeDTO Entity);
        Task<CarMakeDTO> UpdateCarMakeAsync(CarMakeDTO Entity);
        Task DeleteCarMakeAsync(long CarMakeId);
        Task<CarMakeDTO> ToggleActiveStatus(long CarMakeId);
    }
}
