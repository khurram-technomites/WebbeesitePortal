using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierItemCategoryClient
    {
        Task<IEnumerable<SupplierItemCategoryDTO>> GetCategories();
        Task<IEnumerable<SupplierItemCategoryDTO>> GetCategoryByID(long Id);
        Task<SupplierItemCategoryDTO> Create(SupplierItemCategoryDTO model);
        Task<SupplierItemCategoryDTO> Edit(SupplierItemCategoryDTO model);
        Task<SupplierItemCategoryDTO> Delete(long Id);
        Task<SupplierItemCategoryDTO> ToggleActiveStatus(long CityId);
    }
}
