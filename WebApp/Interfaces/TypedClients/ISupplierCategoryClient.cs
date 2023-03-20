using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierCategoryClient
    {
        Task<IEnumerable<SupplierItemCategoryViewModel>> GetAllAsync();
    }
}
