using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantTableReservationService
    {
        Task<IEnumerable<RestaurantTableReservation>> GetAllAsync();
        Task<RestaurantTableReservation> GetByIdAsync(long id);
        Task<RestaurantTableReservation> AddRestaurantTableReservationByOrder(RestaurantTableReservation Model);
        Task<bool> UpdateRestaurantTableReservationByOrder(RestaurantTable Model,long OrderId);
        Task<IEnumerable<RestaurantTableReservation>> GetByRestaurentTableId(long RestaurentTableId);
        Task<IEnumerable<RestaurantTableReservation>> GetReservedByRestaurentTableId(long RestaurentTableId);
        Task<RestaurantTableReservation> AddRestaurantTableReservationAsync(RestaurantTableReservation Model);
        Task<RestaurantTableReservation> UpdateRestaurantTableReservationAsync(RestaurantTableReservation Model);
        Task<RestaurantTableReservation> ArchiveRestaurantTableReservationAsync(long Id);

    }
}
