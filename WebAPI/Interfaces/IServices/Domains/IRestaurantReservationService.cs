using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantReservationService
    {
        Task<IEnumerable<RestaurantReservation>> GetAllAsync();
        Task<RestaurantReservation> GetByIdAsync(long id);
        Task<RestaurantReservation> GetByContactAsync(string contact);
        Task<IEnumerable<RestaurantReservation>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<RestaurantReservation>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<IEnumerable<RestaurantReservation>> GetByStatusAndRestaurantBranchIdAsync(long RestaurantBranchId, string Status);
        Task<RestaurantReservation> AddRestaurantReservationAsync(RestaurantReservation Model);
        Task<RestaurantReservation> UpdateRestaurantReservationAsync(RestaurantReservation Model);
        Task<RestaurantReservation> ArchiveRestaurantReservationAsync(long Id);
    }
}
