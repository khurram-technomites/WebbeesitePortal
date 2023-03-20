using HelperClasses.DTOs.Fatoorah;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Restaurant;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICustomerRestaurantClient
    {
        Task<object> Paid(long orderId , string paymentId);
    }
}
