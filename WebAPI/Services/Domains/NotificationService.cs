using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepo _repo;
        public NotificationService(INotificationRepo repo)
        {
            _repo = repo;
        }
        public async Task<Notification> AddNotification(Notification Entity)
        {
            return await _repo.InsertAsync(Entity);
        }
    }
}
