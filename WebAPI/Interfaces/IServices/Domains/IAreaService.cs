
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IAreaService
    {
        Task<IEnumerable<Areas>> GetAllAreasAsync();
        Task<IEnumerable<Areas>> GetAreaByIdAsync(long Id);
        Task<Areas> AddAreaAsync(Areas Entity);
        Task<Areas> UpdateAreaAsync(Areas Entity);
        Task<Areas> ArchiveAreaAsync(long Id);
    }
}
