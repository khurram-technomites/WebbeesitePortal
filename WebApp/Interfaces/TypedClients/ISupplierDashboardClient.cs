using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierDashboardClient
    {
        Task<SupplierDashboardDTO> GetSupplierDashboardCount(long SupplierId);
    }
}
