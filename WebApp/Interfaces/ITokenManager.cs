using HelperClasses.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces
{
    public interface ITokenManager
    {
        void SetTokenInfo(AccessToken Token);
        String GetAccessToken();
        bool ForcefullyRefreshAccessToken();
    }
}
