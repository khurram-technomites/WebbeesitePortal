using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierItemClient
    {
        Task<IEnumerable<SupplierItemViewModel>> GetBySupplierAsync(long SupplierId);
        Task<SupplierItemViewModel> GetByIdAsync(long Id);
        Task<SupplierItemViewModel> AddAsync(SupplierItemDTO Model);
        Task<SupplierItemViewModel> UpdateAsync(SupplierItemDTO Model);
        Task<SupplierItemViewModel> ToggleActiveStatusAsync(long Id);
        Task<SupplierItemViewModel> ArchiveAsync(long Id);
    }
}
