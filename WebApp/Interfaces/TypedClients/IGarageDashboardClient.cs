using HelperClasses.DTOs.GarageDashboard;
using HelperClasses.DTOs.Supplier;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageDashboardClient
    {
        Task<GarageDashboardStatsDTO> GetGarageDashboardCount(long GarageId);

    }
}
