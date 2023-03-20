using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;

namespace WebAPI.Services.Domains
{
    public class UserRefreshTokenService : IUserRefreshTokenService
    {
        private readonly IUserRefreshTokenRepo _repo;
        public UserRefreshTokenService(IUserRefreshTokenRepo repo)
        {
            _repo = repo;
        }

        public async Task DeleteRefreshTokenAsync(string DeviceTrackId)
        {
            var model = await _repo.GetByIdAsync(x => x.DeviceTrackId == DeviceTrackId);
            await _repo.DeleteAsync(model.FirstOrDefault().Id);
        }
    }
}
