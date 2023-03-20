using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IUserRefreshTokenService
    {
        Task DeleteRefreshTokenAsync(string DeviceTrackId);
    }
}
