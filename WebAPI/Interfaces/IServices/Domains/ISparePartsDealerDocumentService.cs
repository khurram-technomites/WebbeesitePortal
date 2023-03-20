using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartsDealerDocumentService
    {
        Task<IEnumerable<SparePartsDealerDocument>> GetByPath(string Path);

        Task<IEnumerable<SparePartsDealerDocument>> GetAllBySparePartsAsync(long ID);
        Task<IEnumerable<SparePartsDealerDocument>> GetByIdAsync(long Id);
        Task<SparePartsDealerDocument> AddSparePartsDocumentAsync(SparePartsDealerDocument Model);
        Task<SparePartsDealerDocument> UpdateSparePartsDocumentAsync(SparePartsDealerDocument Model);
        Task DeleteSparePartsDocumentAsync(long Id);
        Task DeleteRecord(long Id);
    }
}
