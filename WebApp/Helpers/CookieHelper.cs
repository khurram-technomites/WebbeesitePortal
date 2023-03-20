using HelperClasses.DTOs.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Helpers
{
    public class CookieHelper
    {
        public static Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var userPrincipal = context.Principal;

            String TokenInfo = userPrincipal.Claims.Where(x => x.Type == "AccessTokenInfo")
                                                   .Select(x => x.Value)
                                                   .FirstOrDefault();

            if (string.IsNullOrEmpty(TokenInfo))
            {
                context.RejectPrincipal();
            }
            else
            {
                AccessToken Token = JsonConvert.DeserializeObject<AccessToken>(TokenInfo);

                if (Token.ExpiryDate < DateTime.UtcNow)
                {
                    context.RejectPrincipal();
                }
            }

            return Task.Delay(0);
        }
    }
}
