using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageRatingClient
    {
        Task<IEnumerable<GarageRatingDTO>> GetGarageRatings();
        Task<GarageRatingDTO> ToggleActiveStatus(long RatingId,string status);
        Task<GarageRatingDTO> GetGarageRatingByID(long Id);
    }
}
