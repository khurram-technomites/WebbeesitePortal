using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageRatingService
    {
        Task<GarageRating> AddRatingAsync(GarageRating Model);
        Task<IEnumerable<GarageRating>> GetAllRatingForGarageAsync();
        Task<IEnumerable<GarageRating>> GetGarageRatingByIdAsync(long GarageId );
        Task<GarageRating> UpdateGarageRatingAsync(GarageRating Entity);
    }
}
