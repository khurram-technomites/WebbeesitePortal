using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class FCMUserSessionService : IFCMUserSessionService
    {
        private readonly IFCMUserSessionRepo _repo;
        public FCMUserSessionService(IFCMUserSessionRepo repo)
        {
            _repo = repo;
        }
        public async Task<FCMUserSession> AddUserSession(FCMUserSession Model)
        {
            var list = await _repo.GetByIdAsync(x => x.DeviceId == Model.DeviceId);

            if (list.Any())
                foreach (var record in list)
                    await _repo.DeleteAsync(record.Id);

            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<FCMUserSession>> GetUserSessionTokensByUser(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.UserId == UserId);
        }

    }
}
