using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public  interface ISparePartsDocumentClient
    {
        Task<IEnumerable<SparePartsDealerDocumentDTO>> GetDocumentBySpareParts(long RestaurantId);
        Task<SparePartsDealerDocumentDTO> GetDocumentByID(long Id);
        Task<SparePartsDealerDocumentDTO> AddDocument(SparePartsDealerDocumentDTO model);
        Task<SparePartsDealerDocumentDTO> UpdateDocument(SparePartsDealerDocumentDTO model);

        Task DeleteDocument(long Id);
    }
}
