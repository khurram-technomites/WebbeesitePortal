using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartRequestImagesService
    {
        Task<IEnumerable<SparePartRequestImage>> GetRequestByImageAsync(string Path);
        Task DeleteAsync(long Id);
    } 
}
