using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IServices
{
    public interface ITokenService
    {
        Task<AccessToken> GenerateAccessToken(string UserId, string DeviceTrackId = null);
        Task<AccessToken> RefreshAccessToken(AccessToken Token);
    }
}
