using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface INotificationService
    {
        Task<Notification> AddNotification(Notification Entity);
    }
}
