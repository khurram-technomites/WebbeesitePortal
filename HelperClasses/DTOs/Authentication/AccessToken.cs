using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Authentication
{
    public class AccessToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Issuer { get; set; }

        public AccessToken(string _accessToken, string  _refreshToken, DateTime _expiryDate, string _issuer)
        {
            Token = _accessToken;
            RefreshToken = _refreshToken;
            ExpiryDate = _expiryDate;
            Issuer = _issuer;
        }
    }
}
