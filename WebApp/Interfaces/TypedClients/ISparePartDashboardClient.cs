using HelperClasses.DTOs.SparePartDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartDashboardClient
    {
        Task<SparePartDashboardStatsDTO> GetSparePartDashboardCount(long GarageId);
    }
}
