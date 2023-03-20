using HelperClasses.DTOs;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.RestaurantDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantDashboardClient
    {
        Task<RestaurantDashboardStatsDTO> GetRestaurantDashboardCount(long RestaurantId);
        Task<DashboardStatsDTO> GetPaymentMethodStats(long RestaurantId, long branchId = 0);
    }
}
