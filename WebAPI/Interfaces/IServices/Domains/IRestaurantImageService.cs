using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantImageService
    {
        Task ArchiveRestaurantImageAsync(long Id);
        Task<IEnumerable<RestaurantImages>> GetImageByPath(string Path);
        Task<IEnumerable<RestaurantImages>> GetImageByRestaurant(long Id);
    }
}
