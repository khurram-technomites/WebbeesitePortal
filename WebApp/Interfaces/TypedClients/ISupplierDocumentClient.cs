using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierDocumentClient
    {
        Task<IEnumerable<SupplierDocumentDTO>> GetDocumentBySupplierId(long SupplierId);
        Task<SupplierDocumentDTO> AddSupplierDocument(SupplierDocumentDTO Model);
        Task DeleteSupplierDocument(long Id);
    }
}
