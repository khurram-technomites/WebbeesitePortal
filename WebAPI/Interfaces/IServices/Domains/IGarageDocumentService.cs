using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageDocumentService
    {
        Task<IEnumerable<GarageDocument>> GetByPath(string Path);
        Task<IEnumerable<GarageDocument>> GetByGarage(long GarageId);
        Task<IEnumerable<GarageDocument>> GetByID(long Id);
        Task<GarageDocument> AddDocument(GarageDocument Model);
        Task<GarageDocument> EditDocument(GarageDocument Model);
        Task DeleteRecord(long Id);
    }
}
