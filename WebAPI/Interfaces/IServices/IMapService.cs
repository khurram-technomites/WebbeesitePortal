using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IServices
{
    public interface IMapService
    {
        Task<object> GetPlaces(string Place);
        Task<object> GetPlaceDetails(string PlaceId);
    }
}
