using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IFCMUserSessionService
    {
        Task<IEnumerable<FCMUserSession>> GetUserSessionTokensByUser(string UserId);
        Task<FCMUserSession> AddUserSession(FCMUserSession Model);
    }
}
