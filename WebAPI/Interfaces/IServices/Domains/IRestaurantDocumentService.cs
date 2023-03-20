using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantDocumentService
    {
        Task<IEnumerable<RestaurantDocument>> GetAllByRestaurantAsync(long RestaurantId);
        Task<IEnumerable<RestaurantDocument>> GetByIdAsync(long Id);
        Task<RestaurantDocument> AddRestaurantDocumentAsync(RestaurantDocument Model);
        Task<RestaurantDocument> UpdateRestaurantDocumentAsync(RestaurantDocument Model);
        Task DeleteRestaurantDocumentAsync(long Id);
    }
}
