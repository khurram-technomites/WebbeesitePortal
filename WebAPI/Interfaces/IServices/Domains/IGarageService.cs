using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageService
    {
        Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageFilter Filter);
        Task<long> GetAllGaragesCountAsync();
        Task<long> GetAllGaragesCountByUserIdAsync(long VendorId);
        Task<IEnumerable<Garage>> GetAllGaragesAsync();
        Task<IEnumerable<Garage>> GetGarageByOrigin(string Origin, string Section = "Default");
        IEnumerable<GarageCardResponseDTO> GetAllNearMe(GarageFilter Filter);
        Task<IEnumerable<Garage>> GetGarageByIdAsync(long Id);
        Task<IEnumerable<Garage>> GetGarageBySlugAsync(string Slug);
        Task<IEnumerable<Garage>> GetGarageByLogoAsync(string Path);
        Task<IEnumerable<Garage>> GetGarageByVideoAsync(string Path);
        Task<IEnumerable<Garage>> GetGarageByUserAsync(string UserId);
        Task<IEnumerable<Garage>> GetGarageByVendorAsync(long VendorId);
        Task<Garage> AddGarageAsync(Garage Entity);
        Task<Garage> UpdateGarageAsync(Garage Entity);
        Task<Garage> ArchiveGarageAsync(long Id);
        Task<double> ActiveIactiveCount();
    }
}
