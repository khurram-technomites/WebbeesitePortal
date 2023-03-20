using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IDashboardClient
    {
        Task<AdminDashboardStatsDTO> GetAdminDashboardCount();
        Task<VendorDashboardStatsDTO> GetVendorDashboardCount(long VendorId );
    }
}
