using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantDocumentClient
    {
        Task<IEnumerable<RestaurantDocumentDTO>> GetDocumentByRestaurant(long RestaurantId);
        Task<RestaurantDocumentDTO> GetDocumentByID(long Id);
        Task<RestaurantDocumentDTO> AddDocument(RestaurantDocumentDTO model);
        Task<RestaurantDocumentDTO> UpdateDocument(RestaurantDocumentDTO model);

        Task DeleteDocument(long Id);
    }
}
